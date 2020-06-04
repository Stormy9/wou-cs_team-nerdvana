--select UserBadgeID, UserID as PetopiaUserID, DogProvider, CatProvider, BirdProvider, FishProvider, RodentProvider, RabbitProvider, HorseProvider, LivestockProvider, ReptileProvider, OtherProvider
--from UserBadge;

select UserBadgeID, UserID as PetopiaUserID, 
		DogOwner, CatOwner, BirdOwner, FishOwner, RodentOwner, RabbitOwner, 
		HorseOwner, LivestockOwner, ReptileOwner, OtherOwner
from UserBadge where UserID = 38;

--select * from PetopiaUsers where UserID = 43;
--select * from UserBadge;

-- SO..... we simply HAVE no Providers!!!
-- that is why the query returns nothing..... sigh.

--UPDATE UserBadge SET DogOwner=1, CatOwner=0, BirdOwner=1, FishOwner=0, RodentOwner=0, HorseOwner=0, LivestockOwner=0, ReptileOwner=1, OtherOwner=0 
--WHERE UserID = 9;

--UPDATE UserBadge SET RabbitOwner=0
--WHERE UserID = 15;

--INSERT INTO UserBadge (UserID,DogProvider,CatProvider,BirdProvider,FishProvider,RodentProvider,RabbitProvider,HorseProvider,LivestockProvider,ReptileProvider,OtherProvider)
--VALUES (9, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1)

--INSERT INTO Customers (CustomerName, City, Country)
--VALUES ('Cardinal', 'Stavanger', 'Norway');