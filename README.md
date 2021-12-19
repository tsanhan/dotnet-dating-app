Section 14 summary:
what we've earned:
1. we implemented a 'like user' feature
2. many to many relationship in EF Core 
    * we used a join table for this 
    * in the last ef core versions ef manages to create this relationship automatically
    * even though ef create the join table under the hood, it just invisible to the BE
    * many developers don't trust EF to handle it properly (like me)
    * I like to have control over this relationship

3. configuring entities in the DbContext 
    * this give control over how things are being created in the DB
    * overriding EF conventions  -
    * also called Fluent API, In EF Core, the ModelBuilder class acts as a Fluent API.

up next: implementing a messaging system
