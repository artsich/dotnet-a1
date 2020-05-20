namespace Crawler.Rules
{
    public class DeepRule : AbstractRule, IRule
    {
        public DeepRule(IRule nextRule)
            : base(nextRule)
        {
        }

        public override bool Handle(RuleContext context)
        {
            return Next(context);
        }
    }
}
