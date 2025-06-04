# IEnumerable vs IQueryable in C#

## Question

Why use `IEnumerable<T>` instead of `IQueryable<T>`? What is the best? Why? Explain with example and use context.

## Answer

### Key Differences

1. **IEnumerable<T>**:

   - Works with in-memory collections
   - Executes queries in memory
   - All data is loaded into memory before filtering
   - Good for small datasets or when you need to work with the data in memory

2. **IQueryable<T>**:
   - Works with database queries
   - Executes queries at the database level
   - Only loads the filtered data into memory
   - Better for large datasets and when you need to filter data at the database level

### Current Implementation

In the CityInfo API, the repository interface currently uses `IEnumerable`:

```csharp
public interface ICityInfoRepository
{
    IEnumerable<City> GetCities();
}
```

### Why IQueryable is Better in This Context

1. **Performance**: When you add filters, they are translated to SQL and executed at the database level, reducing the amount of data transferred and processed in memory.

2. **Flexibility**: You can compose queries dynamically. For example:

```csharp
IQueryable<City> query = _context.Cities;

if (filterByName != null)
    query = query.Where(c => c.Name.Contains(filterByName));

if (filterByDescription != null)
    query = query.Where(c => c.Description.Contains(filterByDescription));

// The actual database query is only executed when you enumerate the results
var results = await query.ToListAsync();
```

3. **Pagination**: More efficient when implementing pagination:

```csharp
var pageSize = 10;
var pageNumber = 1;
var cities = await _context.Cities
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

4. **Eager Loading**: Better control over related data loading:

```csharp
var cities = await _context.Cities
    .Include(c => c.PointsOfInterest)
    .Where(c => c.Name.StartsWith("N"))
    .ToListAsync();
```

### Recommended Change

The repository interface should be changed to:

```csharp
public interface ICityInfoRepository
{
    IQueryable<City> GetCities();
    // ... other methods
}
```

### Example Comparison

```csharp
// Current implementation with IEnumerable
public IEnumerable<City> GetCities()
{
    return _context.Cities.ToList(); // Loads ALL cities into memory
}

// If you want to filter cities with name containing "New"
var cities = GetCities().Where(c => c.Name.Contains("New")); // Filtering happens in memory

// Better implementation with IQueryable
public IQueryable<City> GetCities()
{
    return _context.Cities; // Returns query, doesn't execute yet
}

// If you want to filter cities with name containing "New"
var cities = GetCities().Where(c => c.Name.Contains("New")); // Filtering happens at database level
```

### Benefits of Using IQueryable

1. **Deferred Execution**: The actual database query is only executed when you enumerate the results (e.g., by calling `ToList()`, `FirstOrDefault()`, etc.)
2. **Query Composition**: You can build up complex queries efficiently
3. **Database-Level Filtering**: Reduces memory usage and network traffic
4. **Better Performance**: Especially important as your application grows and handles more data

### When to Use Each

- Use `IEnumerable<T>` when:

  - Working with in-memory collections
  - Dealing with small datasets
  - Need to perform operations that can't be translated to SQL

- Use `IQueryable<T>` when:
  - Working with databases
  - Need to filter, sort, or page data
  - Want to optimize performance for large datasets
  - Need to compose dynamic queries
