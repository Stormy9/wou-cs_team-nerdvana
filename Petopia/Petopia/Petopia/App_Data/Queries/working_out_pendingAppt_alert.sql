select TransactionID, CareProviderID, PetID, Pending, Confirmed, Completed_PO, Completed_CP, PC_Comments 
from CareTransaction where CareProviderID = 7;

--UPDATE Customers
--SET ContactName = 'Alfred Schmidt', City= 'Frankfurt'
--WHERE CustomerID = 1;