# BangazonWorkforceManagement
Welcome to **Bangazon Workforce Management,** the employee management system. This application enables HR users to keep track of their employees, departments, computers and employee training programs. Bangazon Workforce is built with C# /.NET MVC and Razor pages for the UI.

## Table of Contents
- [Software Requirements](#software-requirements)
- [Entity Relationship Diagram](#entity-relationship-diagram)
- [Use Instructions](#use-instructions)
- [UI Walkthrough](#human-resources-walkthrough)

## Software Requirements
- Sql Server Manangement Studio
- Visual Studio Community 2017
- Google Chrome

## Enitity Relationship Diagram
<img src="https://i.imgur.com/gxoBsAo.png" />

## Use Intructions
<hr>

Clone this repository.

```
git clone https://github.com/nss-ivory-trolls/WorkforceManagement.git
```

Create a new SQL database. Insert and run the following query:

```
DELETE FROM OrderProduct;
DELETE FROM ComputerEmployee;
DELETE FROM EmployeeTraining;
DELETE FROM Employee;
DELETE FROM TrainingProgram;
DELETE FROM Computer;
DELETE FROM Department;
DELETE FROM [Order];
DELETE FROM PaymentType;
DELETE FROM Product;
DELETE FROM ProductType;
DELETE FROM Customer;


ALTER TABLE Employee DROP CONSTRAINT [FK_EmployeeDepartment];
ALTER TABLE ComputerEmployee DROP CONSTRAINT [FK_ComputerEmployee_Employee];
ALTER TABLE ComputerEmployee DROP CONSTRAINT [FK_ComputerEmployee_Computer];
ALTER TABLE EmployeeTraining DROP CONSTRAINT [FK_EmployeeTraining_Employee];
ALTER TABLE EmployeeTraining DROP CONSTRAINT [FK_EmployeeTraining_Training];
ALTER TABLE Product DROP CONSTRAINT [FK_Product_ProductType];
ALTER TABLE Product DROP CONSTRAINT [FK_Product_Customer];
ALTER TABLE PaymentType DROP CONSTRAINT [FK_PaymentType_Customer];
ALTER TABLE [Order] DROP CONSTRAINT [FK_Order_Customer];
ALTER TABLE [Order] DROP CONSTRAINT [FK_Order_Payment];
ALTER TABLE OrderProduct DROP CONSTRAINT [FK_OrderProduct_Product];
ALTER TABLE OrderProduct DROP CONSTRAINT [FK_OrderProduct_Order];


DROP TABLE IF EXISTS OrderProduct;
DROP TABLE IF EXISTS ComputerEmployee;
DROP TABLE IF EXISTS EmployeeTraining;
DROP TABLE IF EXISTS Employee;
DROP TABLE IF EXISTS TrainingProgram;
DROP TABLE IF EXISTS Computer;
DROP TABLE IF EXISTS Department;
DROP TABLE IF EXISTS [Order];
DROP TABLE IF EXISTS PaymentType;
DROP TABLE IF EXISTS Product;
DROP TABLE IF EXISTS ProductType;
DROP TABLE IF EXISTS Customer;


CREATE TABLE Department (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(55) NOT NULL,
	Budget 	INTEGER NOT NULL
);

CREATE TABLE Employee (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL,
	DepartmentId INTEGER NOT NULL,
	IsSuperVisor BIT NOT NULL DEFAULT(0),
    CONSTRAINT FK_EmployeeDepartment FOREIGN KEY(DepartmentId) REFERENCES Department(Id)
);

CREATE TABLE Computer (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	PurchaseDate DATETIME NOT NULL,
	DecomissionDate DATETIME,
	Make VARCHAR(55) NOT NULL,
	Manufacturer VARCHAR(55) NOT NULL
);

CREATE TABLE ComputerEmployee (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INTEGER NOT NULL,
	ComputerId INTEGER NOT NULL,
	AssignDate DATETIME NOT NULL,
	UnassignDate DATETIME,
    CONSTRAINT FK_ComputerEmployee_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_ComputerEmployee_Computer FOREIGN KEY(ComputerId) REFERENCES Computer(Id)
);


CREATE TABLE TrainingProgram (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(255) NOT NULL,
	StartDate DATETIME NOT NULL,
	EndDate DATETIME NOT NULL,
	MaxAttendees INTEGER NOT NULL
);

CREATE TABLE EmployeeTraining (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	EmployeeId INTEGER NOT NULL,
	TrainingProgramId INTEGER NOT NULL,
    CONSTRAINT FK_EmployeeTraining_Employee FOREIGN KEY(EmployeeId) REFERENCES Employee(Id),
    CONSTRAINT FK_EmployeeTraining_Training FOREIGN KEY(TrainingProgramId) REFERENCES TrainingProgram(Id)
);

CREATE TABLE ProductType (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(55) NOT NULL
);

CREATE TABLE Customer (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(55) NOT NULL,
	LastName VARCHAR(55) NOT NULL
);

CREATE TABLE Product (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	ProductTypeId INTEGER NOT NULL,
	CustomerId INTEGER NOT NULL,
	Price INTEGER NOT NULL,
	Title VARCHAR(255) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	Quantity INTEGER NOT NULL,
    CONSTRAINT FK_Product_ProductType FOREIGN KEY(ProductTypeId) REFERENCES ProductType(Id),
    CONSTRAINT FK_Product_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);


CREATE TABLE PaymentType (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	AcctNumber INTEGER NOT NULL,
	[Name] VARCHAR(55) NOT NULL,
	CustomerId INTEGER NOT NULL,
    CONSTRAINT FK_PaymentType_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id)
);

CREATE TABLE [Order] (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	CustomerId INTEGER NOT NULL,
	PaymentTypeId INTEGER,
    CONSTRAINT FK_Order_Customer FOREIGN KEY(CustomerId) REFERENCES Customer(Id),
    CONSTRAINT FK_Order_Payment FOREIGN KEY(PaymentTypeId) REFERENCES PaymentType(Id)
);

CREATE TABLE OrderProduct (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	OrderId INTEGER NOT NULL,
	ProductId INTEGER NOT NULL,
    CONSTRAINT FK_OrderProduct_Product FOREIGN KEY(ProductId) REFERENCES Product(Id),
    CONSTRAINT FK_OrderProduct_Order FOREIGN KEY(OrderId) REFERENCES [Order](Id)
);

```

Insure that your connection string in appsettings.json is correct.

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR CONNECTION STRING"
  }
}
```

Run the application!




# Human Resources Walkthrough

## Departments

### Department List
To view all Departments, click the "Departments" link in the navigation bar. The Department view shows links for "Create new Department" under the page title and "details" link to the right of each department.

### Create
You can add a new department by clicking the "Create New" link on the Departments List view. The link will show a form with an input field requesting the new department name and budget. Once submitted, you will be returned to the Department List and the new department will be displayed.

### Details
Click on the "Details" link on an individual department to see a list of that department's employees.


## Employees 

### Employee List
To see a list of employees, click the "Employees" link in the navbar. You will be presented with a table displaying the First Name, Last Name, and Department Name of each employee, as well as a checkbox indicating whether or not the employee is a supervisor.

### Create
You can add a new employee by clicking "Create New" link on the View all Employees view. The link will show a form with an input field requesting the new employee's first name, last name, supervisor status, department, and a submit button. Once submitted they will be rerouted back to view all Employee page and the new employee is also listed.

### Details
When the employee list is being viewed, the user will be able to click on the "Details" link that will bring them to a detailed view of the specific employee that was associated with the "Details" link. The user should see the employee's first name, last name, full name, department name, and a list of training programs they are enrolled in with the details of the program listed as well. The user should see an "Edit" button at the bottom.

### Edit
When the "Edit" link next to an employee (or on the bottom of the "Details" page) is clicked, it will direct the user to a form for editing employee details. The link will show a form with input fields where the user can change the basic information (first name, last name, supervisor status, department).  The user is also able to change the employee's assigned computer and training programs the employee is signed up for, both enrolling and unrolling the employee from training.  At the bottom the form contains a submit button. Once submitted the form will be refreshed with the newly changed data.  When the user is done editing an employee, they can click the "Back to List" link at the bottom of the page or the "Employees" link in the navbar to return to the list of all employees. 



## Computers

### Computer List
To see the Computer Index view, click on the Computer tab in the navbar. You will see a table that has a column for Make, Model and Employee Assigned. Each of these columns will be filled with the corresponding information that is sourced from the database. On the right of each row, for each Computer, you should see a hyperlink for "Detail."

### Create
From the Computers List, click the Create New link to show a form with corresponding computer input fields.  Once the fields have been filled, use the submit button to create a new computer (and assign it to an employee if desired).  Once submitted you will be returned to the Computers list.

### Details
Click on the Make of an indiviual computer in the Index view. The browser will show the details of the selected computer. The details will include the date of purchase of the computer, date the computer was decommissioned if applicable, the employee assigned to the computer and the model and manufacturer.

### Delete 
On the details view for all Computers page, click on the the delete link. The user will be shown a view with the computer details and will ask for confirmation of the delete. Once the user clicks on delete the computer is deleted and the user is taken back to the index view or if the computer is currently assigned or has been previously assign the delete is denied.


## Training Programs

### Program List
To see a list of upcoming training programs, click on the "Training Programs" link in the navbar. You'll then see a table listing each program's Name, Start Date, End Date, and Max Attendees. To see a list of _past_ training programs, click the "View Past Programs" at the top of the list page.

### Create
To create a new training program, click the "Create New Program" link at the top of the List page. You will need to input the Name, Start Date, End Date, and Max Number of Attendees for the new program. When done, click Save and you will be returned to the Training Program list.

### Details
To see the details of a training program, click the "Detail" link on the List page. In addition to the basic program information, you will also see a list of employees who are attending or attended that program.

### Edit
If viewing the details page of an _upcoming_ training program, you will see a "Edit" link. When you click "Edit," you will be be presented with a form to update the detail of that program. After clicking "Save," you will be returned to the training programs list page. If you are viewing a _past_ training program, you will not be able to edit that program.

### Delete
If viewing the details page of an _upcoming_ training program, you will see a "Delete" link. When you click "Delete," you will be asked to confirm the deletion. If you then click the "Delete" button, the program will be deleted from the system and you will be returned to the training programs list. If you are viewing a _past_ training program, you will not be able to delete that program.

