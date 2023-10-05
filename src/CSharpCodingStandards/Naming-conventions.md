# Naming conventions

This document contains pre-defined usage of certain names for C# classes. These names are reserved for their specified purpose and should be always be followed.

The list is not final and is expected to keep improving.

## Adapter

**Description**: Adapter classes allow the integration of incompatible or legacy systems into new architectures by providing a consistent interface.

**Examples**:

- `LegacySystemAdapter`: Adapts an old system to work seamlessly with modern components.
- `ExternalLibraryAdapter`: Wraps an external library to provide a standardized interface for use in an application.

## Attribute

**Description**: Attributes are used to add metadata or behavior to code elements like classes, methods, or properties.

**Examples**:

- `ValidationAttribute`: Used for custom data validation in models.
- `AuthorizeAttribute`: Custom authorization attribute for controlling access to controller actions.

## Builder

**Description**: Builder classes facilitate the construction of complex objects step by step, providing a fluent and expressive API.

**Examples**:

- `FormBuilder`: Helps construct complex form objects in a user interface.
- `ReportBuilder`: Creates structured report objects with configurable settings.

## Controller

**Description**: Controllers are often used in web applications to handle incoming HTTP requests, route them to the appropriate actions, and manage the application's flow.

**Examples**:

- `HomeController`: Manages the main entry point for a web application.
- `ProductController`: Handles product-related operations in a web application.

## DTO (Data Transfer Object)

**Description**: DTOs are used for transferring data between layers or services in a distributed system to minimize the number of calls.

**Examples**:

- `UserDTO`: Contains data to be transferred between client and server for user-related operations.
- `OrderDTO`: Represents data to be passed between different parts of an application.

## Exception

**Description**: Exception classes are used to represent errors or exceptional conditions in code.

**Examples**:

- `DatabaseException`: Represents an exception related to database operations.
- `ConfigurationException`: Indicates an issue with configuration settings.

## Factory

**Description**: Factories are responsible for creating instances of complex objects, providing a way to centralize object creation logic.

**Examples**:

- `ConnectionFactory`: Creates database connections or resources.
- `WidgetFactory`: Generates instances of widgets or objects.

## Helper/Utility

**Description**: Helper or utility classes contain reusable methods and functions that don't fit into specific categories.

**Examples**:

- `StringUtils`: Provides utility methods for string manipulation.
- `FileHelper`: Contains file-related utility functions.

## Manager

**Description**: Manager classes coordinate or manage various components or services in an application.

**Examples**:

- `CacheManager`: Coordinates caching operations in an application.
- `EventManager`: Manages event-related functionality.

## Model

**Description**: Models represent data structures and often correspond to database tables or entities.

**Examples**:

- `UserModel`: Represents a data model for users, typically used with databases.
- `ProductModel`: Represents a data model for products.

## Provider

**Description**: Provider classes offer access to specific services, resources, or data sources, often abstracting the details of acquisition.

**Examples**:

- `AuthenticationProvider`: Manages user authentication services.
- `DataProvider`: Offers access to various data sources in a standardized way.

## Repository

**Description**: Repositories are used for data access and abstract database operations.

**Examples**:

- `CustomerRepository`: Manages data access for customer-related entities in a database.
- `OrderRepository`: Handles data access for orders.

## Service

**Description**: Services provide specific application services or business logic.

**Examples**:

- `EmailService`: Contains methods for sending emails.
- `LoggingService`: Manages logging operations in an application.

## Singleton

**Description**: Singleton classes ensure that a single instance of the class exists throughout the application's lifecycle.

**Examples**:

- `ConfigurationSingleton`: Represents a configuration manager as a single, shared instance.
- `LoggerSingleton`: Provides a centralized logging mechanism with a single, shared logger instance.

## Strategy

**Description**: Strategy classes define interchangeable algorithms or behaviors, allowing dynamic selection of strategies.

**Examples**:

- `PaymentStrategy`: Implements different payment processing strategies.
- `SortingStrategy`: Contains algorithms for sorting data.

## ViewModel

**Description**: ViewModels are used in applications with a separation of concerns like MVVM (Model-View-ViewModel) to represent view-specific data for display.

**Examples**:

- `UserViewModel`: Represents a view-specific model for user-related data in a web application.
- `ProductViewModel`: Represents a view-specific model for product data.