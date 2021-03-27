# coin-combo

This program implements a recursive solution to the [subset sum problem](https://en.wikipedia.org/wiki/Subset_sum_problem). Without optimisations, the time complexity is `O(3^n)`. The total number of nodes in the recursion tree is approximately `(3^(n+1))/2`. However, pruning and memoisation significantly reduce the size of the tree.

The hashtable requires `O(2n)` memory. Considering the hashtable significantly reduces execution time, the trade-off here is highly favourable. 

This [video lecture](https://www.youtube.com/watch?v=kyLxTdsT8ws) explains the problem very well.

## Compile

Targets .NET 5.0

```powershell
> dotnet build
```

## Run

```powershell
> dotnet run
```
