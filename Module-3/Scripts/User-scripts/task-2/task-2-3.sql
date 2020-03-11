
select * from Northwind.Employees
--#1 
--......The first task has invalid description 

--#2
select Customers.ContactName, count(Orders.OrderID) as 'Amount'
from Northwind.Customers as Customers
left join Northwind.Orders as Orders on Orders.CustomerID = Customers.CustomerID
group by Customers.ContactName
order by Amount