using StringFormatter.Core.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StringFormatter.Core
{
    public class StringFormatter1
    {
        public readonly static StringFormatter1 Shared = new StringFormatter1();

        private ConcurrentDictionary<Type, ConcurrentDictionary<string, Func<object, string>>> _cache = new();

        public string Format(string template, object target)
        {
            var tokens = Tokinezer.GetTokens(template);

            if (!_cache.ContainsKey(target.GetType()))
                _cache.TryAdd(target.GetType(), new());

            foreach (ReplaceToken tokenToReplace in tokens.Where(x => x is ReplaceToken))
            {
                if (!_cache[target.GetType()].ContainsKey(tokenToReplace.TokenText))
                {
                    //MemberInfo member = GetFieldOrProperty(target.GetType(), tokenToReplace.ClearFieldName);

                    Func<object, string> func = GetExpression(target.GetType(), tokenToReplace.ClearFieldName);

                    _cache[target.GetType()].TryAdd(tokenToReplace.TokenText, func);
                }

                tokenToReplace.Replacement = _cache[target.GetType()][tokenToReplace.TokenText](target);
            }

            return string.Join("", tokens);
        }

        private static MemberInfo GetFieldOrProperty(Type target, string name)
        {
            MemberInfo? memberInfo = target.FindMembers(
                MemberTypes.Field | MemberTypes.Property,
                BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Instance,
                (x, y) => x.Name == y.ToString(),
                name
            ).FirstOrDefault();

            if (memberInfo == null)
                throw new Exception("No such public field or property");

            return memberInfo;
        }

        private static Func<object, string> GetExpression(Type target, string asccesField)
        {
            ParameterExpression numParam = Expression.Parameter(typeof(object), "target");
            Expression cast = Expression.Convert(numParam, target);
            MemberExpression call = Expression.PropertyOrField(cast, asccesField);
            Expression toString = Expression.Call(call, "ToString", null);
            Expression<Func<object, string>> lambda1 =
                Expression.Lambda<Func<object, string>>(
                    toString,
                    new ParameterExpression[] { numParam });

            return lambda1.Compile();
        }
    }
}
