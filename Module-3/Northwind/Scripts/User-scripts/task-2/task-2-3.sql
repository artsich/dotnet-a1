--#1 
select distinct EmployeeTerritories.EmployeeID
from Northwind.EmployeeTerritories as EmployeeTerritories
join Northwind.Territories as Territory
join Northwind.Region as Region 
on Region.RegionID = Territory.RegionID
on EmployeeTerritories.TerritoryID = Territory.TerritoryID
where Region.RegionDescription='Western'

--#2
select Customers.ContactName, count(Orders.OrderID) as 'Amount'
from Northwind.Customers as Customers
left join Northwind.Orders as Orders on Orders.CustomerID = Customers.CustomerID
group by Customers.ContactName
order by Amount