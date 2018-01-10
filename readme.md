# ServiceFabric.ECommerce

This project is based on the Pluralsight course ['Understanding the Programming Models of Azure Service Fabric'](https://app.pluralsight.com/library/courses/azure-service-fabric-programming-models/table-of-contents) by Ivan Gavryliuk.

Instead of just following the course and creating the project along, I've done some modifications that are -in my humble opinion- improvements:

- The CheckoutService is not responsible for retrieving the shopping-cart history of a user.  I've delegated this to the UserActor which now keeps track of his shopping-cart history.

- The communication between the CheckoutService and the ProductCatalogService is less chatty:  
When checking out items, the CheckoutService will not call the ProductCatalogService for each product in the cart seperately.  Instead, the CheckoutService will call the ProductCatalogService only once and retrieves the Product-details for every product-id that is present in the shopping cart in one go.

- The TotalPrice property on the CheckoutSummary model is a  calculated property instead of a get / set property.