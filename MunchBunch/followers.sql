-- Get all users other than current user that the current user isn't already following.
select FirstName || LastName 'Full Name', PrimaryLocation 'Location'
from AspNetUsers au
left join UserFollow uf on au.Id = uf.UserId
where au.Id is not "f7a1d774-5b91-40c6-a69b-6c0fa633e970" and
uf.FollowerId is not "f7a1d774-5b91-40c6-a69b-6c0fa633e970";

