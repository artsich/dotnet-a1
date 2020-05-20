using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler.Rules
{
    public interface IRule
    {
        bool Handle(RuleContext context);
    }
}
