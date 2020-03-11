select * from Northwind.[Order Details];

--#1
select distinct OrderId from Northwind.[Order Details] as Details
where Details.Quantity between 3 and 10

--#2 
select CustomerId, Country from Northwind.Customers
where Country between 'b' and 'h'
order by Country;

--#3 
select CustomerId, Country from Northwind.Customers
where Country >= 'b' and Country < 'h'
order by Country;