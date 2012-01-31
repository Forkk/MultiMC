import java.io.File;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Modifier;

public class MultiMCLauncher {
	/**
	 * @param args The arguments you want to launch Minecraft with. New path, Username, Session ID.
	 */
	@SuppressWarnings("unchecked")
	public static void main(String[] args) {
		if(args.length != 3)
			System.exit(-1);
		try {
			@SuppressWarnings("rawtypes")
			Class mc = Class.forName("net.minecraft.client.Minecraft"); // Get the Minecraft Class.
			if(mc == null)
				throw new ClassNotFoundException(); // This shouldn't ever be called, but just in case.
				Field[] fields = mc.getDeclaredFields();

				for (int i = 0; i < fields.length; i++) {
					Field f = fields[i];
					if (f.getType() != File.class) { // Has to be File.
						continue;
					}
					if (f.getModifiers() != Modifier.PRIVATE + Modifier.STATIC) { // And Private Static.
						continue;
					}
					f.setAccessible(true);
					f.set(null, new File(args[0])); // And set it.
					System.out.println("Fixed Minecraft Path: Field was "
							+ f.toString());
				}
				
				String[] mcArgs = new String[2];
				mcArgs[0] = args[1];
				mcArgs[1] = args[2];
				
				mc.getMethod("main", String[].class).invoke(null, (Object)mcArgs);
			}
		catch(ClassNotFoundException e)
		{
			System.exit(1);
		} catch (IllegalArgumentException e) {
			System.exit(2);
		} catch (IllegalAccessException e) {
			System.exit(2);
		} catch (InvocationTargetException e) {
			System.exit(3);
		} catch (NoSuchMethodException e) {
			System.exit(3);
		} catch (SecurityException e) {
			System.exit(4);
		}
	}

}
