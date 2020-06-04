select * from CareProvider where CareProviderID = 7;
---------------------------------------
select * from PetopiaUsers where UserID = 15 OR UserID = 14;
-------------------------------------------
select TransactionID, CareTransaction.PetOwnerID, PetopiaUsers.UserID, PetID, CareProviderID, StartDate, StartTime, EndDate, EndTime 
from CareTransaction join PetOwner on PetOwner.PetOwnerID = CareTransaction.PetOwnerID
				     join PetopiaUsers on PetopiaUsers.UserID = PetOwner.UserID where TransactionID = 81;
-----------------------------------------------
select * from CareTransaction where CareProviderID = 7 and PetOwnerID = 9 and TransactionID = 81;