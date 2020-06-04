--select UserBadgeID, UserID as PetopiaUserID, DogProvider, CatProvider, BirdProvider, FishProvider, RodentProvider, RabbitProvider, HorseProvider, LivestockProvider, ReptileProvider, OtherProvider
--from UserBadge;

--select * from PetopiaUsers where UserID = 43;
--select * from UserBadge;

-- SO..... we simply HAVE no Providers!!!
-- that is why the query returns nothing..... sigh.

--UPDATE UserBadge SET BirdProvider=0, FishProvider=0, RodentProvider=0, HorseProvider=0, LivestockProvider=0, ReptileProvider=0, OtherProvider=0 WHERE UserID = 39;

INSERT INTO UserBadge (UserID,DogProvider,CatProvider,BirdProvider,FishProvider,RodentProvider,RabbitProvider,HorseProvider,LivestockProvider,ReptileProvider,OtherProvider)
VALUES (9, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1)

--INSERT INTO Customers (CustomerName, City, Country)
--VALUES ('Cardinal', 'Stavanger', 'Norway');