using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Zorro.Data.Attributes;
using Zorro.Data.Interfaces;
using Zorro.Extensions;

namespace Zorro.Data;

using Inclusion = string[];
using InclusionContext = IDictionary<string, bool?>;

public class ModelRepository<TEntity>
    where TEntity : class, IEntity
{
    private DbSet<TEntity> set = default!;

    private Dictionary<int, Inclusion> contextBasedInclusion = new();
    public const int NULL_CONTEXT_INCLUSION_HASH = 0;
    public const int MAX_INCLUSION_DEPTH = 3;

    public DbContext context { get; init; } = null!;

    public ModelRepository(DbContext context, ref DbSet<TEntity> set)
    {
        this.context = context;
        this.set = set;

        CalculateAndStoreInclusion(null, NULL_CONTEXT_INCLUSION_HASH, false);
    }

    public Inclusion CalculateAndStoreInclusion(
        InclusionContext? inclusionContext,
        int contextHash,
        bool containsHash
    )
    {
        Inclusion inclusion = CalculateInclusion(inclusionContext);

        if (contextHash == NULL_CONTEXT_INCLUSION_HASH && !containsHash)
        {
            contextBasedInclusion.Add(NULL_CONTEXT_INCLUSION_HASH, inclusion);
        }
        else if (contextHash == NULL_CONTEXT_INCLUSION_HASH && containsHash)
        {
            contextBasedInclusion[NULL_CONTEXT_INCLUSION_HASH] = inclusion;
        }
        else if (contextHash != NULL_CONTEXT_INCLUSION_HASH && !containsHash)
        {
            contextBasedInclusion.Add(contextHash, inclusion);
        }
        else if (contextHash != NULL_CONTEXT_INCLUSION_HASH && containsHash)
        {
            contextBasedInclusion[contextHash] = inclusion;
        }

        return inclusion;
    }

    private Inclusion CalculateInclusion(InclusionContext? inclusionContext)
    {
        List<string> result = new();

        void ProcessType(Type type, string prefix, int currentDepth)
        {
            if (currentDepth > MAX_INCLUSION_DEPTH)
                return;

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var setInclusionDepthAttr = prop.GetCustomAttribute<InclusionDepthAttribute>();
                if (setInclusionDepthAttr is not null)
                {
                    if (currentDepth >= setInclusionDepthAttr.DEPTH)
                        continue;
                }

                bool hasAlwaysIncludeAttr = prop.GetCustomAttribute<AlwaysIncludeAttribute>() is not null;
                bool hasNeverIncludeAttr = prop.GetCustomAttribute<NeverIncludeAttribute>() is not null;

                bool hasAnyVariableAttr = prop.GetCustomAttributes<IncludeWhenAttribute>().Any();
                bool shouldInclude = false;

                if (isNavigationType(prop.PropertyType, out bool isIEnumerable))
                {
                    if (hasAlwaysIncludeAttr)
                        shouldInclude = true;
                    else if (hasNeverIncludeAttr)
                        shouldInclude = false;
                    else if (hasAnyVariableAttr)
                    {
                        var whenAttributes = prop.GetCustomAttributes<IncludeWhenAttribute>();
                        shouldInclude = whenAttributes.All(IsVariableAttrSatisfiesContext);
                    }
                }

                if (shouldInclude)
                {
                    string path = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}.{prop.Name}";
                    result.Add(path);

                    Type nextType = isIEnumerable && prop.PropertyType.IsGenericType
                        ? prop.PropertyType.GetGenericArguments().FirstOrDefault() ?? typeof(object)
                        : prop.PropertyType;

                    if (typeof(IEntity).IsAssignableFrom(nextType))
                    {
                        ProcessType(nextType, path, currentDepth + 1);
                    }
                }
            }
        }

        bool isNavigationType(Type propType, out bool isIEnumerable)
        {
            bool isIEntity = typeof(IEntity).IsAssignableFrom(propType);
            isIEnumerable = typeof(IEnumerable).IsAssignableFrom(propType) && propType != typeof(string);

            return isIEntity || isIEnumerable;
        }

        bool IsVariableAttrSatisfiesContext(IncludeWhenAttribute attr)
        {
            if (inclusionContext is null)
                return attr.MAY_EXPECT_NULL;

            bool hasVariable = inclusionContext.ContainsKey(attr.VARIABLE_NAME);
            if (!hasVariable)
                return attr.MAY_EXPECT_NULL;

            bool? contextVariableValue = inclusionContext[attr.VARIABLE_NAME];
            if (contextVariableValue is null)
                return attr.MAY_EXPECT_NULL;
            else
                return contextVariableValue == attr.EXPECTED_VALUE;
        }

        ProcessType(typeof(TEntity), "", 0);
        return result.ToArray();
    }


    private Inclusion GetContextInclusion(InclusionContext? inclusionContext)
    {
        int contextHash = inclusionContext.GetContentBasedHashCode(NULL_CONTEXT_INCLUSION_HASH);

        if (contextBasedInclusion.ContainsKey(contextHash))
            return contextBasedInclusion[contextHash];
        else
            return CalculateAndStoreInclusion(inclusionContext, contextHash, false);
    }

    public bool Save()
    {
        int stateEntries = context.SaveChanges();
        return stateEntries > 0;
    }

    public TEntity? Add(TEntity instance, InclusionContext? inclusionContext = null)
    {
        var entry = set.Add(instance);
        return Save() ? FindById(entry.Entity.Id, inclusionContext) : null;
    }

    public bool Update(ref TEntity instance, InclusionContext? inclusionContext = null)
    {
        var entry = set.Update(instance);
        bool updateStatus = Save();

        instance = FindById(entry.Entity.Id, inclusionContext)!;

        return updateStatus;
    }

    public bool Remove(TEntity instance)
    {
        set.Remove(instance);
        return Save();
    }

    public bool Clear()
    {
        set.RemoveRange(set);
        return Save();
    }

    public int Count()
    {
        return set.Count();
    }

    public IEnumerable<TEntity> GetAll(
        InclusionContext? inclusionContext = null
    )
    {
        return set
            .NoTrackingInclude(GetContextInclusion(inclusionContext))
            .ToList();
    }

    public IEnumerable<TEntity> GetAll(
        Expression<Func<TEntity, bool>> predicate,
        InclusionContext? inclusionContext = null
    )
    {
        return set
            .NoTrackingInclude(GetContextInclusion(inclusionContext))
            .Where(predicate)
            .ToList();
    }

    public TEntity? Find(
        Expression<Func<TEntity, bool>> predicate,
        InclusionContext? inclusionContext = null
    )
    {
        return set
            .NoTrackingInclude(GetContextInclusion(inclusionContext))
            .FirstOrDefault(predicate);
    }

    public TEntity? FindById(
        int id,
        InclusionContext? inclusionContext = null
    )
    {
        return set
            .NoTrackingInclude(GetContextInclusion(inclusionContext))
            .FirstOrDefault(item => item.Id == id);
    }

    public IEnumerable<TEntity> FindByIds(
        IList<int> ids,
        InclusionContext? inclusionContext = null
    )
    {
        return set
            .NoTrackingInclude(GetContextInclusion(inclusionContext))
            .Where(item => ids.Contains(item.Id))
            .ToList();
    }
}