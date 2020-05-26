--select TransactionID, CareProviderID, PetID, PetOwnerID, NeededThisVisit, Pending, Confirmed, Completed_PO, Completed_CP, 
--		PC_Rating, PC_Comments, PO_Rating, PO_Comments 
--from CareTransaction where PetOwnerID = 9;

update CareTransaction SET Completed_PO=1, Completed_CP=0, PO_Rating=NULL, PO_Comments=NULL WHERE TransactionID=110;