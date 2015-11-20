# NetDotNet
Full platform for object-oriented .NET web development

This platform includes both a webserver and API written in C#.

The webserver is designed to allow you to: 

1. Decide how files are served to the client 
  2. Streamed from the disk 
  1. Kept in memory 
2. Customize error pages, even making them dynamic 

The API is designed to allow you to:

1. Write simple, close-to-the-core, dynamic content generators in an object-oriented style
2. Create building blocks like top and side bars that can be reused by all generators, avoiding repeated code/html.
3. Treat HTML as a hierarchy of objects that can be reused and dynamically constructed with the Object Initializer pattern in C#
