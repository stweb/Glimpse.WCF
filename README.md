About
=====

A plugin to track WCF calls in Glimpse and show them on the timeline.
The requests are captured via GlimpseWcfClientInspector which implements IClientMessageInspector.

[![Build status](https://ci.appveyor.com/api/projects/status/lgwb2hkcvyohvfdf)](https://ci.appveyor.com/project/stweb/glimpse-wcf)

Usage
=====

- Reference Glimpse.WCF and its dependencies (from NuGet)
- Register WCF Behaviour via code

    using Glimpse.Wcf;
	clientRuntime.MessageInspectors.Add(new GlimpseWcfClientInspector());

Reference
=========

http://msdn.microsoft.com/en-us/library/aa717047(v=vs.110).aspx
http://blogs.msdn.com/b/carlosfigueira/archive/2011/04/19/wcf-extensibility-message-inspectors.aspx
http://www.universalthread.com/ViewPageArticle.aspx?ID=191