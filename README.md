EasyScript
==========

This project aims at lowering the entry barrier for creating nice and re-usable scripts for
SpaceEngineers. It builds upon the [MDK-SE](https://github.com/malware-dev/MDK-SE) by Malware,
by offering an opinionated structure and helpers for common tasks.

How to use
----------

- [Download](https://github.com/spectras/se-scripts/archive/master.zip) the project.
- Copy-paste the `EasyScript` directory into your project.
- Inside Visual Studio:
	- right click on your project in the solution explorer.
	- select "Add → Reference..." from the menu.
	- in the Shared Projects group, tick `EasyScript`.

A first script
--------------

You no longer write your `Program` directly. Instead, you will be writing the `MainScript`, and
EasyScript will call the appropriate functions at the right time.

Let's look at this scripts that shows a counter on all screens of the ship:

```csharp
class MainScript : MyMainScript
{
	int Counter = 0;
	List<IMyTextPanel> Screens = new List<IMyTextPanel>();

	public override void onLoad(MyIni data) {
		Runtime.UpdateFrequency = UpdateFrequency.Update100;
	}

	public override void ScanBlocks(IMyGridTerminalSystem grid)	{
		grid.GetBlocksOfType<IMyTextPanel>(Screens, Block.OnGridOf(Me))
	}
	
	public override void onTick(UpdateType source) {
		Counter += 1;
		foreach (var screen in Screens) {
			screen.ContentType = ContentType.TEXT_AND_IMAGE;
			screen.WriteText(Counter.ToString());
		}
	}
}
```

Here is what each part does:
- **onLoad:** is called when the world has finished loading and the script configuration has
  been checked and it is ready to run. Use it to perform initialization. Here we just set our
  script to run every 100 ticks of game time.
- **ScanBlocks:** is called whenever you should rescan the grid. We use it here to get all
  screens that are on the same grid/subgrid than the ProgrammableBlock.
- **onTick:** is called at the frequency defined in `onLoad`. Here we go through the screen list
  and write the counter on each.

Try to run it.

Try to add a screen after starting the counter: it will not update. This is because ScanBlocks
is not called automatically for performance reason. But you can make it trigger easily: simply
click "run" with no argument, or run the PB from a timer with no argument.

Keeping the counter in saved game
---------------------------------

Easy: we will need to implement `onSave` and modify `onLoad`.

First, to avoid duplicating things, let's give a name to the save counter:

```csharp
// At the top of MainScript class
static readonly string SaveVersion = "1";
static readonly MyIniKey CounterKey = new MyIniKey(SaveVersion, "counter");
```

We will use it to remember current counter value when the game is saved:
```csharp
public override void onSave(MyIni data)
{
	data.Set(CounterKey, Counter);
}
```

And modify `onLoad` to actually load the value from the saved game:
```csharp
public override void onLoad(MyIni data) {
	Counter = data.Get(CounterKey).ToInt32(0);
	Runtime.UpdateFrequency = UpdateFrequency.Update100;
}
```
**Note:** `ToInt32` converts the value to an `int`, the type we used for our `Counter`.
    There are also `ToIn64()` for `long`, `ToString()` for `string`, … The value in the
	parenthesis will be used as default value if no saved value was found.

Allow resetting the counter
---------------------------

We want now to let the user reset the counter manually, or using a button or sensor.

We simply add a function to handle the command:

```csharp
public override void onCommand(MyCommandLine args)
{
	if (args.Argument(0) == "reset") { Counter = 0; }
}
```

That's it! You can now reset the counter by calling the ProgrammableBlock with argument `reset`,
from the terminal menu or from some trigger.

**Done!** You can review the full code in [EasyCounter/Program.cs](EasyCounter/Program.cs)
