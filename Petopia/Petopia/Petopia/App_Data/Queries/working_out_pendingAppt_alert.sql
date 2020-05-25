select TransactionID, CareProviderID, PetID, PetOwnerID, Pending, Confirmed, Completed_PO, Completed_CP, PC_Comments 
from CareTransaction where PetOwnerID = 9;

--UPDATE Customers SET ContactName = 'Alfred Schmidt', City= 'Frankfurt' WHERE CustomerID = 1;

--update CareTransaction SET Completed_PO = 1 WHERE TransactionID = 112;