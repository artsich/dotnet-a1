namespace Crawler.Rules
{
    public class IgnorredContextRule : AbstractRule, IRule
    {
        private string[] _ignorredFileExts;

        public IgnorredContextRule(string[] ignorredFileExts, IRule nextRule) 
            : base(nextRule)
        {
            _ignorredFileExts = ignorredFileExts;
        }

        public override bool Handle(RuleContext context)
        {


            return Next(context);
        }
    }
}
