[![Build status](https://ci.appveyor.com/api/projects/status/8gfs2jgf3ec334r4?svg=true)](https://ci.appveyor.com/project/sebagomez/rochaissuesbot)

![Genexus Bender](./res/GenexusBender.png)
Genexus Issue Tracking Bot
==========================

Genexus Issue tracking bot is a bot developed with the newly released [Microsoft Bot Framework](https://dev.botframework.com/).

It adds integration with Language Undertanding Intelligent Service ([Luis](http://luis.ai)) for understanding natural language queries to the internal Genexus Issue tracking system.

It has a couple of commands like **/list** and **/search** that implement the [FormBuilder](http://docs.botframework.com/sdkreference/csharp/de/d9d/class_microsoft_1_1_bot_1_1_builder_1_1_form_flow_1_1_form_builder.html) in order to guide the user through the proccess.

It is now working in [slack](http://slack.com) [facebook](http://www.facebook.com/genexusitbot) and [Telegram](http://telegram.org). It was rejected from Skype because of the icon, so I won't publish it there.

Change log:

- 27-Aug-2016 - Upgraded to bot framework v3