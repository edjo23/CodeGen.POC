# Overview
This POC was created to investigate what would be required to enable the use of MS DI extensions with BEEF. In addition using an alternative off the shelf code generation library has been included as part of the POC.

# Dependency injection
The goal is to enable the use of DI so that we can get the benefits of DI. To make that happen the following changes were made.

## DataSvc as instance
Data service layer generated as a non-static so that it can be used with DI.

## Option to make constructors private
With MS DI types can only have a single public constructor. To allow for a custom constructor in the custom partial the gen'ed constructor either needs to be omitted or made private. The following code then works.

```
public partial class PersonDataSvc
{
    public PersonDataSvc(IPersonData data, IConfiguration configuration) : this(data)
    {
    }
}
```

## Service collection helpers
Service collection extension methods created to aid in the MS DI setup.

# Code generation
The goal is to see if an alternative code generator could help make BEEF easier to maintain.

Another goal was to see what effort would be required to change the code generator.

## Handlebars
Handlebars is used for code generation as it is simple. The configration of the code generation process though can in theory support multiple code generation libraries and is not limited to one.

## Model
Handlebars has limited logic controls so requires a richer model to bind to. This is seen as a beneift though as all logic is code based so can be easily maintained and debugged.

The main entity model has reasonable explicit data for all classes generated: 

```
Entity
- EntityClass
- DataInterface
- DataClass
- DataServiceInterface
- DataServiceClass
- ManagerInterface
- ManagerClass
- ControllerClass
```

Other models can be created and used as required.

## Work in progress
This is a work in progress, there are many missing features.

## Extensibility
DI was used to configure the code generation. The intent was to try and make it easier to extend the code generation process to allow for other artifacts to be generated, e.g. markdown files, mermaid diagrams.

Addition generators can be added by projects if required, along with custom models.

## Configurtion
Uses MS configuration extension to allow generator config to be easily overridden. E.g. templates, output targets, other properties.

## YAML entity config
YAML was used for the entity config. Due to the separation of config loading and model creation, in theory the format used could be user definable.

## Other minor things

### Optional partial classes
The option to enable/disable partial classes was added. Originally this was an idea to tie partial classes to private constructors (for enabling DI), but that idea failed but the concept was left in place. One advantage is that if partials are not required the generated code is cleaner.

### Nullable reference types
There are some differences to nullable types.
