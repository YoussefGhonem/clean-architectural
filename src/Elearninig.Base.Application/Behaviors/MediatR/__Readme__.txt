

•	When a request is sent through MediatR, it goes through a series of pipeline behaviors
•	enabling you execute validation or logging logic before and after your Command or Query Handlers execute,

TRequest: TRequest is a generic type parameter used to represent the request message in MediatR. 
It can be any class or struct that represents a request for some operation to be performed. 
TRequest does not enforce any specific interface or base class, allowing flexibility in defining request types.

IRequest<TResponse>: IRequest<TResponse> is an interface provided by MediatR that represents a request message with a corresponding response.
It is a marker interface with no members of its own. 
Implementing IRequest<TResponse> indicates that a class is a request type and expects a response of type TResponse.

------------------------------------------------------------------------------------------------


Compare Between behaviors and middlewares
##########################################

Both behaviors and middlewares in ASP.NET Core provide ways to intercept and modify the request/response pipeline,
but they have different purposes and usage patterns.

Behaviors:

Behaviors are typically used in conjunction with MediatR or similar request/response handling libraries.
Behaviors are applied at the application level or specific to a set of requests.
Behaviors operate on the individual requests and responses handled by the library.
Behaviors are primarily used to implement cross-cutting concerns, such as validation, logging, exception handling, performance monitoring, authorization, etc.
Behaviors are executed before and/or after the main business logic of the request is executed.
Behaviors have access to the request and response objects as well as other services via dependency injection.
Behaviors can be composed together to form a pipeline, allowing multiple behaviors to be executed in a specific order.
Behaviors are typically registered and configured during the application startup phase.

Middlewares:

Middlewares are components that can be inserted into the request/response pipeline in a specific order.
Middlewares are applied globally or per request.
Middlewares operate at a lower level in the pipeline and have direct access to the HttpContext.
Middlewares are executed sequentially in the order they are added to the pipeline.
Middlewares can perform a wide range of tasks such as authentication, routing, compression, static file serving, logging, etc.
Middlewares are responsible for handling the request, modifying the response, or passing the request to the next middleware in the pipeline.
Middlewares have access to the HttpContext and can manipulate the request and response objects as well as other services via dependency injection.
Middlewares are typically registered and configured during the application startup phase.
In summary, behaviors are primarily used in the context of request/response handling libraries and 
focus on implementing cross-cutting concerns at the application or request level. Middlewares,
on the other hand, operate at a lower level in the pipeline and provide more 
fine-grained control over the request/response flow, allowing for a wide range
of tasks to be performed. Both behaviors and middlewares have their own use cases
and can be used together to achieve the desired behavior and functionality in an ASP.NET Core application.