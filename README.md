# MultitaskingDebugDemo

This solution demonstrates various parallel programming techniques in C# for use with Visual Studio's **Debug Windows**: Tasks, Threads, Parallel Stacks, and Parallel Watch.

---

## Techniques Used

- `Task.Run`
- `await TaskAsync`
- `Parallel.For`
- `Parallel.Invoke`
- LINQ `.AsParallel()`

---

## Breakpoint Suggestions

Place breakpoints in the following methods to observe parallel behavior:

- `CPUIntensiveTask()`
- `IOBoundTask()`
- `Log()`
- Lambdas in `Parallel.For` and `Parallel.Invoke`
- LINQ `.AsParallel()` query

---

## Output
![Tasks Window](Documentation/Images/CommandWindow.JPG)

---

## Debug Windows

### 🔹 Tasks
View active, scheduled, or completed `Task` objects and their IDs.

![Tasks Window](Documentation/Images/TasksWindow.JPG)

### 🔹 Threads
Inspect thread pool threads, their IDs, and call stacks.

![Threads Window](Documentation/Images/ThreadsWindow.JPG)

### 🔹 Parallel Stacks
Visualize how threads are forked and joined during parallel execution.

![Parallel Stacks](Documentation/Images/ParallelStack.JPG)

### 🔹 Parallel Watch
Observe variable values across multiple threads or tasks.

![Parallel Watch](Documentation/Images/ParallelWatch.JPG)
