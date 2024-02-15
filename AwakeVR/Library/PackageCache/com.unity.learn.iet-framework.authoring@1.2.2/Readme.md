# Tutorial Authoring Tools
---------
This package makes tooling available to be able to create Tutorials.

## Setup
For Unity 2021.2 and newer, simply search for "Tutorial Authoring Tools" in the Package Manager. For older Unity versions, this package is not currently discoverable,
and you need to add the following line to the `dependencies` list of `Packages/manifest.json`:  
`"com.unity.learn.iet-framework.authoring": "1.2.1"`

When using the authoring tools, it's best to have both Tutorial Framework and Tutorial Authoring Tools set explicitly as the dependencies of your project, for example:

    {
        "dependencies": {
            "com.unity.learn.iet-framework": "3.1.1",
            "com.unity.learn.iet-framework.authoring": "1.2.1"
        }
    }

Make sure that the packages have compatible versions, which most likely are the latest versions of each package.

## Authoring tools
This package allows you to create new Assets related to Tutorials:

- Tutorial Container
- Tutorial
- Tutorial Page
- Tutorial Styles
- Tutorial Welcome Page
- Tutorial Project Settings

It will also enable the authoring toolbar at the top of the **Tutorials** window, allowing to author and test the tutorials more easily.
