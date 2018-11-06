-- Get all users other than current user that the current user isn't already following.
select FirstName || LastName 'Full Name', PrimaryLocation 'Location'
from AspNetUsers au
left join UserFollow uf on au.Id = uf.UserId
where au.Id is not "3bc119c1-212e-4829-ba4c-bed4c2cf7f50" and
uf.FollowerId is not "3bc119c1-212e-4829-ba4c-bed4c2cf7f50" and
FirstName like '%Dar%' or LastName like "%Dar%" or PrimaryLocation like "%Dar%";

