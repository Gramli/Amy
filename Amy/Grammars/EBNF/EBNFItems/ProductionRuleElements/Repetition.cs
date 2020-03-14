using Amy.Caching;
using System.Collections.Generic;
using System.Text;

namespace Amy.Grammars.EBNF.EBNFItems.ProductionRuleElements
{
    /// <summary>
    /// EBNF Repetition rule (group)
    /// </summary>
    internal class Repetition : IGroupProductionRule
    {
        public const string notation = "{";
        public const string endNotation = "}";
        public string Notation => Repetition.notation;
        public string EndNotation => Repetition.endNotation;

        public bool IsOptional => true;

        private readonly IEBNFItem _item;

        private SmartFixedCollection<string> _cache;

        public Repetition(IEBNFItem item, int cacheLength)
        {
            this._item = item;
            this._cache = new SmartFixedCollection<string>(cacheLength);
        }

        /// <summary>
        /// Resolve value using by chars concat
        /// </summary>
        /// <returns></returns>
        public bool IsExpression(string value)
        {
            var result = string.IsNullOrEmpty(value) || this._cache.Contains(value);
            if (!result)
            {
                var builder = new StringBuilder();
                for (var i = 0; i < value.Length -1; i++)
                {
                    builder.Append(value[i]);
                    if (this._item.IsExpression(builder.ToString()))
                    {
                        var ii = i + 1;
                        var restOfValue = value[ii..];
                        result = IsExpression(restOfValue);
                        if(result)
                        {
                            this._cache.Add(value);
                        }
                        break;
                    }
                }
            }
            if(!result && (this._item.IsExpression(value) && !this._cache.Contains(value)))
            {
                this._cache.Add(value);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Rebuild repetition rule with item like is defined in grammar
        /// </summary>
        /// <returns></returns>
        public string Rebuild()
        {
            return $"{this.Notation}{this._item.Rebuild()}{this.EndNotation}";
        }

        public IEnumerable<IExpressionItem> ExpressionStructure(string value)
        {
            IEnumerable<IExpressionItem> result = null;
            if (IsExpression(value)) result = this._item.ExpressionStructure(value);
            return result;
        }
    }
}