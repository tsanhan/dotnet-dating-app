Using TypeScript:
so far we have used ts as js and used 'any' as type there we needed.

in brief:
ts gives us intellisense/autocomplete fir the types we have defined and used.

create a test.ts  and past this in:

```
//1. will work on js, not ts
let a = 1;
a = "s";

 
// if we want a to be a string or a number, we need to specify a type (union type in this case) 
let b: string | number = 1;
b = "s";


//2. lets say we want to have two objects
const car1 = {
    color: 'red',
    model: 'bmw'
}
const car2 = {
    color: 'blue',
    model: 'kia',
    topSpeed: 100
}

// we can hover and see the type' thats great.
// but usually we want an interface to have our objects in order.

// lets create an interface for our car to have type safety:
interface Car {
    color: string;
    model: string;
    topSpeed: number; 
}

// ts will alert us if we missing something or not following the interface directive
// we can hover and see the error
// we can make the topSpeed be optional (adding ?), but it does't mean it can be a string
const car3:Car = {
    color: 'red',
    model: 'bmw'
}
const car4:Car = {
    color: 3,
    model: 'kia',
    topSpeed: 100
}


// 3. functions:
// by hovering we can see ts thinks x,y are 'any' type but the function returns a number (because of the *)
// if we specify x as a string (x: string) ts wil know our method will have a problem
// we can be explicit about what the function returns (: number)
const multiply = (x, y) => x * y;


```

we'll be using ts from now on.

delete the test.ts file


