package openpool.pocket;

import gnu.io.CommPort;
import gnu.io.CommPortIdentifier;
import gnu.io.SerialPort;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.Arrays;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

import processing.core.PApplet;

public class OpenpoolBoostPocket {

	public static final int DATABITS_5 = 5;
	public static final int DATABITS_6 = 6;
	public static final int DATABITS_7 = 7;
	public static final int DATABITS_8 = 8;

	public static final int STOPBITS_1 = 1;
	public static final int STOPBITS_2 = 2;
	public static final int STOPBITS_1_5 = 3;

	public static final int PARITY_NONE = 0;
	public static final int PARITY_ODD = 1;
	public static final int PARITY_EVEN = 2;
	public static final int PARITY_MARK = 3;
	public static final int PARITY_SPACE = 4;

	private PApplet pa;
	private ScheduledExecutorService ses;
	private CommPort port;
	private String portName;
	private Future<?> future;
	private boolean isDebug;
	
	private int[] pockets;
	private int[] totalPockets;

	private InputStream in;
	private OutputStream out;

	public OpenpoolBoostPocket(PApplet pa, String portName) {
		this.pa = pa;
		this.portName = portName;
		pockets = new int[6];
		totalPockets = new int[6];
		pa.registerDispose(this);
	}

	public void start() {
		ses = Executors.newSingleThreadScheduledExecutor();
		Executors.newSingleThreadExecutor().execute(new Runnable() {
			public void run() {
				connect(115200,
						OpenpoolBoostPocket.DATABITS_8,
						OpenpoolBoostPocket.STOPBITS_1,
						OpenpoolBoostPocket.PARITY_NONE);
				startMeasurement();
				waitForLEDs();
				startPolling();
			}
		});
	}

	private boolean connect(int dataRate, int dataBits, int stopBits, int parity) {
		if (in != null) {
			return true;
		}

		// Open the port.
		try {
			final CommPortIdentifier portIdentifier = CommPortIdentifier.getPortIdentifier(portName);
			port = portIdentifier.open("class", 2000);
			((SerialPort) port).setSerialPortParams(dataRate,
					dataBits,
					stopBits,
					parity);
			in = port.getInputStream();
			out = port.getOutputStream();
		} catch (Exception e) {
			disconnect();
			return false;
		}
		return true;
	}

	private void disconnect() {
		if (port != null) {
			port.close();
			port = null;
		}
		in = null;
		out = null;
	}
	
	public void stop() {
		ses.execute(new Runnable() {
			public void run() {
				stopPolling();
			}
		});
	}
	
	public void resetTotal() {
		Arrays.fill(totalPockets, 0);
	}
	
	public int[] getTotalPockets() {
		return this.totalPockets.clone();
	}
	
	public int[] getPockets() {
		int[] pockets = this.pockets.clone();
		Arrays.fill(this.pockets, 0);
		return pockets;
	}
	
	public void setDebug(boolean isDebug) {
		this.isDebug = isDebug;
	}
	
	public void dispose() {
		pa.unregisterDispose(this);
		stop();
	}
	
	private void startMeasurement() {
		try {
			out.write('s');
			String message = readString();
			if (isDebug) {
				System.out.print("pocket detector: ");
				System.out.println(message);
			}
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	private void waitForLEDs() {
		try {
			Thread.sleep(1000);
		} catch (InterruptedException e) {
			// Do nothing.
		}
	}

	private void startPolling() {
	    future = ses.scheduleAtFixedRate(new Runnable() {
	        public void run() {
	            try {
	                poll();
	            } catch (IOException e) {
	                e.printStackTrace();
	            }
	        }
	    }, 33, 33, TimeUnit.MILLISECONDS);
	}

	private void stopPolling() {
	    future.cancel(false);
	}

	private void poll() throws IOException {
	    out.write('r');
	    
	    String message = readString().trim();
	    String[] pocketStrings = message.split(",");
	    
	    for (int pocket = 0; pocket < 6 && pocketStrings.length == 6; pocket++) {
	        try {
	            
	            int numPocket = Integer.parseInt(pocketStrings[pocket].trim());
	            pockets[pocket] += numPocket;
	            totalPockets[pocket] += numPocket;
	        } catch (NumberFormatException nfe) {
	            if (isDebug) {
	                System.err.print("pocket detector: "
	                        + "something wrong happened when "
	                        + "parsing info at pocket no.");
	                System.err.println(pocket);
	                System.err.println(message);
	            }
	        }
	    }
	}

	private String readString() throws IOException {
		StringBuilder sb = new StringBuilder();

		int b = 0;
		while ((b = in.read()) >= 0) {
			if (b == '\n') break;
			sb.append((char) b);
		}

		return sb.toString();
	}

}
