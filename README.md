## COM Guid Problem Analyser for ReSharper ##

This plugin provides a problem analyser for the [`assembly:Guid`](http://msdn.microsoft.com/en-us/library/system.runtime.interopservices.guidattribute.aspx) attribute, highlighting when a non-unique guid is found.
Also provides a quick fix to generate a new Guid.

For ReSharper 8
-
You can use the new NuGet-based [Extension Manager](http://www.jetbrains.com/resharper/whatsnew/index.html#extension_manager), select **ReSharper - Extensions Manager**, and grab for a package called [**ReSharper.ComGuidChecker**](https://resharper-plugins.jetbrains.com/packages/ReSharper.ComGuidChecker/).

For ReSharper 7:
- 
- Errr.. Sorry. Upgrade :)

## Bugs? Questions? Suggestions?

Please feel free to [report them](../../issues)!

Thanks to [Igal Tabachnik](https://twitter.com/hmemcpy) - this work is based a lot on his [Resharper.InternalsVisibleTo](https://resharper-plugins.jetbrains.com/packages/ReSharper.InternalsVisibleTo) package.