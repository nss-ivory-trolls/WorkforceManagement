# BangazonAPI
Welcome to **Bangazon!** The new virtual marketplace. This marketplace allows customers to buy and sell their products through 
a single-page application webpage and its data is tracked through a powerful, hand-crafted and solely dedicated API. 

## Table of Contents
- Software Requirements
- Entity Relationship Diagram
- Database Setup
- Http Request Methods:
1. Customers
1. Products
1. Payment Types
1. Orders
1. Product Types
1. Employees
1. Departments
1. Computers
1. Training Programs


## Software Requirements
- Sql Server Manangment Studio
- Visual Studio Community 2017
- Postman
- Google Chrome

## Enitity Relationship Diagram
<img src="erd.png" width="900" />

## Database Setup
(to be added)

## Http Request Methods

## 1. CUSTOMERS

### GET

* select `GET` then paste `localhost:5000/customers` into the field and click send. The result should be an array of all the orders in the database.

* select `GET` then paste `localhost:5000/customers/?_include=payments` into the field and click send. The result should be an array of all the customers in the database with all of the payment types included in that customers as well.

* select `GET` then paste `localhost:5000/customers/?_include=products` into the field and click send. The result should be an array of all the customers in the database with all of the products types included in that customers as well.

* select `GET` then paste `localhost:5000/customers/?q=bo` into he field and click send. The result should be an array of all the customers in the database with first or last names that contains sat.

* select `GET` then paste `localhost:5000/customers/1` or any other number that showed up in the previous query as CustomerId and click send. The result should be only that object of the specified Customer

* select `GET` then paste `localhost:5000/customers/2?_include=payments` into the field and click send. The result should be a specific customer in the database with all of the payment types included to that customer as well.

* select `GET` then paste `localhost:5000/customers/2?_include=products` into the field and click send. The result should be a specific customer in the database with all of the products types included to that customer as well.

* select `GET` then paste `localhost:5000/customers/2?_active=false` into the field and click send.  The result should be an array of customers that do not have active orders.

* select `GET` then paste `localhost:5000/customers/2?_active=true` into the field and click send.  The result should be an array of customers that currently have active orders.

### POST

select `POST` then paste `localhost:5000/customers` into the field, then click Body underneath the field, then select raw, and then paste this snippet or make one similar

```
{
	"FirstName": "Testing",
	"LastName": "OneTwo"
   }
```
then click send. The result should be the new customer you made.

### PUT

select `PUT` then paste `localhost:5000/customers` or any other `Customer Id`, then click Body underneath the field, then select raw, and then paste this snippet or make one similar
```
{
	"FirstName": "Testing",
	"LastName": "NewLastName"
   }
```

You should get nothing back from this. When you run the `GET` query the Customer you specified in your `PUT` query should show the updated, edited information you gave it.

## 2. PAYMENT TYPES

Use the command `dotnet run` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:

### GET

To GET all product types, select GET in Postman then paste `localhost:5000/PaymentTypes` into the field and click send. The result should be an array of all the payment type in the database that should look like:

```
[
{
"paymentTypeId": 1,
   "paymentTypeName": "Visa",
   "AcctNumber": 1,
   "CustomerId": 2
 },
 {
   "paymentTypeId": 2,
   "paymentTypeName": "MasterCard",
   "AcctNumber": 1,
   "CustomerId": 2
 },
 {
   "paymentTypeId": 3,
   "paymentTypeName": "Discover",
   "AcctNumber": 1,
   "CustomerId": 2
}
]
```

To GET a specific, single payment type, add an /{id} to the end of the `localhost:5000/PaymentTypes` URL. The result should only include the single payment type with the Id you added like the below:


```
[
{
"paymentTypeId": 3,
   "paymentTypeName": "Discover",
   "AcctNumber": 1,
   "CustomerId": 2
}
]
```

### POST

To POST a new object to your existing array for PaymentType, select POST, then paste `localhost:5000/PaymentTypes` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new PaymentType you made:

```
{
	"paymentTypeName": "New Value"
}
```
### PUT

To update an existing PaymentType, select PUT then paste `localhost:5000/paymentTypes/2` or any other existing order. Then follow the same directions as the POST example, and change the values then click send:
```
{
	"paymentTypeName": "New Value"
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.

### DELETE

To DELETE an existing product type, select DELETE then paste `localhost:5000/PaymentTypes/2` or any other existing PaymentType then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.

## 3. PRODUCTS

Use the command `dotnet run` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:

### GET

To GET all product types, select GET in Postman then paste `localhost:5000/Products` into the field and click send. The result should be an array of all the products in the database that should look like:

```
[
{
    "Id": 1,
   "Price": 1,
   "Title": "neato",
   "Description": "Rad",
   "Quantity": 2,
   "ProductTypeId": 2,
   "CustomerId": 2
 },
 {
   "Id": 2,
   "Price": 21,
   "Title": "aTitle,
   "Description": "Rad",
   "Quantity": 2,
   "ProductTypeId": 2,
   "CustomerId": 2
 },
 {
   "Id": 3,
   "Price": 1,
   "Title": "stuff",
   "Description": "Rad",
   "Quantity": 2,
   "ProductTypeId": 2,
   "CustomerId": 2
}
]
```
To GET a specific, single product type, add an /{id} to the end of the l`ocalhost:5000/products` URL. The result should only include the single product  with the Id you added like the below:

```
[
{
   "Id": 3,
   "Price": 1,
   "Title": "stuff",
   "Description": "Rad",
   "Quantity": 2,
   "ProductTypeId": 2,
   "CustomerId": 2
}
]
```
### POST

To POST a new object to your existing array for Product, select POST, then paste `localhost:5000/Products` into the field. Then click Body underneath the field, select raw, and then paste this below snippet or make one similar then click send. The result should be the new Product you made:

```
{
	"Product": "New Value"
}
```
### PUT

To update an existing PaymentType, select PUT then paste `localhost:5000/products/2` or any other existing order. Then follow the same directions as the POST example, and change the values then click send:
```
{
	"paymentTypeName": "New Value"
}
```
You should get nothing back from this besides an OK status. When you run the GET query the computer you specified in your PUT query should show the updated, edited information you gave it.

### DELETE

To DELETE an existing product type, select DELETE then paste `localhost:5000/products/2` or any other existing product then click send. You should get nothing back from this besides an OK status. When you run the GET query the order with the Id you specified in your DELETE query should no longer exist.

## 4. Orders

### GET

To GET all Orders, select GET in Postman, then input `localhost:5000/orders` into the field and click Send. The result should be an array of all the Orders in the database.

To GET a single order, add an Id to the end of the `localhost:5000/orders` URL, e.g. `localhost:5000/orders/2`. The result should only include the single order with the Id you queried.

Query Parameters:
- To return a list of products in each order, add `?_include=products` to the URL.
- To return the name of the customer for each order, add `?_include=customers` to the URL.
- To see all completed orders, add `?_completed=true` to the URL.
- To see all uncomplete orders, add `?_completed=false` to the URL.

### POST

To POST a new Order object, select POST in Postman, then input `localhost:5000/orders` into the field. Then click Body, select Raw, paste in the snippet below, and click Send. The result should be the new Order you made.

```
{
   "CustomerId": 2
}
```

### PUT

To update an existing order, select PUT, then input `localhost:5000/order/2` or the Id of another existing order. Then click Body, select Raw, paste in the snippet below, and click Send.

```
{
   "PaymentTypeId": 1
}
```

This should result in a "200 OK" status. You can run another GET query to see the item you updated.

### DELETE

To DELETE an existing Order, select DELETE, then input `localhost:5000/orders/3` or the Id of another existing Order and click Send. You should get a "200 OK" status. If you run a GET query using the Id you specified in your DELETE query, that item should no longer exist.

## 5. Product Types

Use the command `dotnet run` to start the program, BangazonAPI. Once the program is running, open up the Postman desktop app and run the following commands for each request method:

### GET

To GET all ProductTypes, select GET in Postman, then input `localhost:5000/producttype` into the field and click Send. The result should be an array of all ProductTypes in the database that should look like this:

```
[
    {
        "id": 1,
        "name": "Electronics"
    },
    {
        "id": 2,
        "name": "Sports Equipment"
    },
    {
        "id": 3,
        "name": "Furniture"
    }
]
```

To GET a specific ProductType, add an Id to the end of the `localhost:5000/producttype` URL, e.g. `localhost:5000/producttype/2`. Only the single ProductType with the Id you added should display once you click Send, like below:

```
[
    {
        "id": 1,
        "name": "Electronics"
    }
]
```

### POST

To POST a new object to your existing array for ProductType, select POST, then input `localhost:5000/producttype` into the field. Then click Body, select Raw, paste in the snippet below, and click Send. The result should be the new ProductType you made:

```
{
	"name": "Health & Beauty"
}
```

### PUT

To update an existing ProductType, select PUT, then input `localhost:5000/producttype/2` or the Id of another existing ProductType. Then click Body, select Raw, paste in the snippet below, and click Send. 

```
{
    "id": 2,
    "name": "Sports"
}
```

This should result in a "200 OK" status. You can run another GET query to see the item you updated.

### DELETE

To DELETE an existing ProductType, select DELETE, then input `localhost:5000/producttype/4` or the Id of another existing ProductType and click Send. You should get a "200 OK" status. If you run a GET query using the Id you specified in your DELETE query, that item should no longer exist.


## 6. Employees

### GET

### POST

### PUT

### DELETE

## 7. Departments

### GET

### POST

### PUT

### DELETE

## 8. Computers

### GET

### POST

### PUT

### DELETE

## 9. Training Programs

### GET

### POST

### PUT

### DELETE
