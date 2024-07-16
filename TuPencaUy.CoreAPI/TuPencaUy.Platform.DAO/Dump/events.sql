set identity_insert Event on; 
Insert into Event (Id, Name, StartDate, EndDate, Comission, Instantiable, Inactive, CreationDate, LastModificationDate, TeamType, Finished) values
(1, 'Eurocopa', getdate(), DATEADD(month, 2, getdate()), 0.08, 1, 0, GETDATE(), GETDATE(), 1, 0),
(2, 'Copa américa', DATEADD(day, 3, getdate()), DATEADD(month, 1, DATEADD(day, 3, getdate())), 0.05, 1, 0, GETDATE(), GETDATE(), 1, 0),
(3, 'NBA Playoffs', DATEADD(day, 10, getdate()), DATEADD(day, 45, DATEADD(day, 10, getdate())), 0.1, 1, 0, GETDATE(), GETDATE(), 2, 0)
set identity_insert Event off; 
Insert into EventSport (EventsId, SportsId) values
(1, 1), (2, 1), (3, 2)