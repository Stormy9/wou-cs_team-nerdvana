select TransactionID as T_ID, CareProviderID as CP_ID, PetID, PetOwnerID as POID, 
		Pending, Confirmed, Completed_PO as Cmp_PO, Completed_CP as Cmp_PC, PC_Rating, PO_Rating 
from CareTransaction;
-----------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------
select avg(PC_Rating) as CarerRating, avg(PO_Rating) as OwnerRating from CareTransaction;