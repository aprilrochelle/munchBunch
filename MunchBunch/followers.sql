-- Get all users other than current user that the current user isn't already following.
select au.Id 'Id', au.FirstName 'First Name', au.LastName 'Last Name', au.PrimaryLocation 'Location'
from AspNetUsers au
left join UserFollow uf on au.Id = uf.ReceivingUserId
where au.Id is not "66c1b20a-fb18-4278-9381-8bcc18b9eaa1" and
uf.RequestingUserId is not "66c1b20a-fb18-4278-9381-8bcc18b9eaa1" and
FirstName like '%Apr%' or LastName like "%Apr%" or PrimaryLocation like "%Apr%"
group by au.Id;

