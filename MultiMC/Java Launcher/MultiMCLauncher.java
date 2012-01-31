import java.io.File;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Modifier;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLClassLoader;

public class MultiMCLauncher
{
	/**
	 * @param args
	 *            The arguments you want to launch Minecraft with. New path,
	 *            Username, Session ID.
	 */
	public static void main(String[] args)
	{
		if (args.length != 3)
		{
			System.out.println("Not enough arguments.");
			System.exit(-1);
		}
		
		try
		{
			String[] jarFiles = new String[] {
				"minecraft.jar", "lwjgl.jar", "lwjgl_util.jar", "jinput.jar"
			};
			
			URL[] urls = new URL[jarFiles.length];
			
			for (int i = 0; i < urls.length; i++)
			{
				try
				{
					File f = new File(new File(args[0], "bin"), jarFiles[i]);
					urls[i] = f.toURI().toURL();
					System.out.println("Loading URL: " + urls[i].toString());
				} catch (MalformedURLException e)
				{
//					e.printStackTrace();
					System.err.println("MalformedURLException, " + e.toString());
					System.exit(5);
				}
			}
			
			System.out.println("Setting natives");
			System.setProperty("java.library.path", 
					new File(new File(args[0], "bin"), "natives").toString());
			System.out.println("java.library.path set to " + 
					System.getProperty("java.library.path"));
			
			System.out.println("Loading jars...");
			URLClassLoader cl = new URLClassLoader(urls, MultiMCLauncher.class.getClassLoader());
			
			// Get the Minecraft Class.
			Class<?> mc = cl.loadClass("net.minecraft.client.Minecraft");
			Field[] fields = mc.getDeclaredFields();
			
			for (int i = 0; i < fields.length; i++)
			{
				Field f = fields[i];
				if (f.getType() != File.class)
				{
					// Has to be File
					continue;
				}
				if (f.getModifiers() != (Modifier.PRIVATE + Modifier.STATIC))
				{
					// And Private Static.
					continue;
				}
				f.setAccessible(true);
				f.set(null, new File(args[0]));
				// And set it.
				System.out.println("Fixed Minecraft Path: Field was "
						+ f.toString());
			}
			
			String[] mcArgs = new String[2];
			mcArgs[0] = args[1];
			mcArgs[1] = args[2];
			
			mc.getMethod("main", String[].class).invoke(null, (Object) mcArgs);
		} catch (ClassNotFoundException e)
		{
			System.err.println("ClassNotFoundException, " + e.toString());
			System.exit(1);
		} catch (IllegalArgumentException e)
		{
			System.err.println("IllegalArgumentException, " + e.toString());
			System.exit(2);
		} catch (IllegalAccessException e)
		{
			System.err.println("IllegalAccessException, " + e.toString());
			System.exit(2);
		} catch (InvocationTargetException e)
		{
			System.err.println("InvocationTargetException, " + e.toString());
			System.exit(3);
		} catch (NoSuchMethodException e)
		{
			System.err.println("NoSuchMethodException, " + e.toString());
			System.exit(3);
		} catch (SecurityException e)
		{
			System.err.println("SecurityException, " + e.toString());
			System.exit(4);
		}
	}
	
}
