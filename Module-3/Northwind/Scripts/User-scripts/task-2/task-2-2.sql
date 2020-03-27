--select count(distinct Orders.OrderID) as 'Count' from Northwind.Orders

--#1
select 
	datepart(yyyy, Orders.OrderDate) as 'Year ', 
	count(Orders.OrderID) as 'Count'
from Northwind.Orders as Orders
group by datepart(yyyy, Orders.OrderDate);

--#2
select 
	concat(Employees.FirstName, ' ', Employees.LastName) as Seller,
	count(Orders.OrderID) as 'Amount' 
from Northwind.Orders as Orders
join Northwind.Employees as Employees on Employees.EmployeeID = Orders.EmployeeID
group by concat(Employees.FirstName, ' ', Employees.LastName)
order by Amount desc

--#3
select
	Orders.EmployeeID,
	Orders.CustomerID,
	count(Orders.OrderID) as 'Qty'
from Northwind.Orders as Orders
where datepart(yyyy, Orders.OrderDate) = 1998
group by Orders.EmployeeID, Orders.CustomerID
order by Orders.EmployeeID

--#4
select Employees.EmployeeID, Employees.City, Customers.City, Customers.CustomerID
from Northwind.Employees, Northwind.Customers
where Employees.City = Customers.City

--#5
select c1.ContactName, c2.ContactName, c1.City
from Northwind.Customers as c1, Northwind.Customers as c2
where c1.City = c2.City and c1.CustomerID != c2.CustomerID
