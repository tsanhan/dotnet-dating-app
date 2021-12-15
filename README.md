Restoring the caching for members:
so things are a bit more complicated now after we implemented filtering and pagination.
now we need to remember the filter and page so we wont refetch the data, right?
ok so we'll do it nice and slow.
we'll be caching members results, so go to members.service.ts 

ok so we have caching in the members page.
what about the  member details pages? this is bit more complicated, we don't have a list of members, now we have a map of members, and we need to extract the member from the map.

next up: Restoring caching for member details
