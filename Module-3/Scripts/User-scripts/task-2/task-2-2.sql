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
select 