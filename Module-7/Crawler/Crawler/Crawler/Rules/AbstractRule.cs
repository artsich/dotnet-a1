namespace Crawler.Rules
{
    public abstract class AbstractRule : IRule
    {
        private IRule _nextRule;

        protected AbstractRule(IRule nextRule)
        {
            _nextRule = nextRule;
        }

        public abstract bool Handle(RuleContext context);

        protected bool Next(RuleContext context)
        {
            return _nextRule?.Handle(context) ?? false;
        }
    }
}
