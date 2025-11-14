# MovieRental Exercise

This is a dummy representation of a movie rental system.
Can you help us fix some issues and implement missing features?

- The app is throwing an error when we start, please help us. Also, tell us what caused the issue.

  **Answer**: The issue is that MovieRentalDbContext will create one instance for every request (Scoped) while we are trying to define it as Singleton in the Program configurations, and Singleton services cannot depend on Scoped services because they will change per request.

- The rental class has a method to save, but it is not async, can you make it async and explain to us what is the difference?

  **Answer**: Currently the code will first add the rental to the Rental DBSet, and afterwards it will try to save the changes into the database. Without asnyc/await, this means that our thread will now be blocked while waiting for the database operation to finish before continuining the code; with async/await, when the request to make changes is made, the thread is released until the operation is done, meaning better efficient use of resources.

- Please finish the method to filter rentals by customer name, and add the new endpoint.

- We noticed we do not have a table for customers, it is not good to have just the customer name in the rental.
  Can you help us add a new entity for this? Don't forget to change the customer name field to a foreign key, and fix your previous method!

  **Answer**: A new entity was created (Customer), afterwards EF needs to be used to add the migration and update the database. Fixed the previous method in a simple direct way, but there should exist additional protections like making sure the customerId is valid and that a Customer exists for that id, before getting the rentals for the customer.

- In the MovieFeatures class, there is a method to list all movies, tell us your opinion about it.

  **Answer**: The problem is related to getting all the movies from the database into memory, if we have a large database then we are going to occupy a large size of memory with the movies data, it would be best to add pagination so we can get a smaller group of movies at a time.

- No exceptions are being caught in this api, how would you deal with these exceptions?

  **Answer**: Most often, Exceptions should not be caught in the API, they should be managed by the middleware (we can create our own for this). We should only catch exceptions in the API when we intend to hide domain information (so, catch and rethrow with different data) or when we can recover from the error (retry an operation). We can also deal with expected errors, like validation errors, so we can return a more specific information regarding the error.

      ## Challenge (Nice to have)

  We need to implement a new feature in the system that supports automatic payment processing. Given the advancements in technology, it is essential to integrate multiple payment providers into our system.

Here are the specific instructions for this implementation:

- Payment Provider Classes:
  - In the "PaymentProvider" folder, you will find two classes that contain basic (dummy) implementations of payment providers. These can be used as a starting point for your work.
- RentalFeatures Class:
  - Within the RentalFeatures class, you are required to implement the payment processing functionality.
- Payment Provider Designation:
  - The specific payment provider to be used in a rental is specified in the Rental model under the attribute named "PaymentMethod".
- Extensibility:
  - The system should be designed to allow the addition of more payment providers in the future, ensuring flexibility and scalability.
- Payment Failure Handling:

  - If the payment method fails during the transaction, the system should prevent the creation of the rental record. In such cases, no rental should be saved to the database.

  **Answer**: I created a IProviderPayment interface, for the existing payment providers to implement, as this will allow more payment providers to be added in future, without need to make existing code. To get the payment provider we use the rental.paymentmethod property, which has the name of the provider to use. In this simple example we are just creating a new instance of the corresponding payment provider, but in a more complex scenario we should instead create scoped instances of the payment providers to use in these requests, so that the system handles the resource managemenet more effectively.
  When saving a rental, we first get the payment provider and verify if it exists, before attempting to make payment. If payment is a success, then we save the rental. We could have more validation protection (making sure values exist) and more detailed information for when the operation fail with custom exceptions (like FailedPaymentException), that our middleware would later handle.
