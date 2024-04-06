# TGC.Core

This package consists of standard functionality such as errorhandling, serialization and such. Any TGC solution should use the functionality from this package if available.

The different areas of functionality can individually be added to the service collection via its extension method.

## Exception handling

## Serialization

## Changelog

### 0.1.0
Updated inner workings of exception handling, added factory to throw exceptions and added default Http exception handling.

### 0.0.2
Fixed modifier bug and introduced configurability of json serializer through IServiceCollection injection

### 0.0.1
First version for preview of core functionality