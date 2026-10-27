using Gee.External.Capstone.PowerPc;
using Gee.External.Capstone;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static racman.rac2;
using Keystone;
using Newtonsoft.Json.Linq;

namespace racman
{ 
    public partial class MemoryForm : Form
    {
        public static uint mobyInstancesAddr;
        public static uint mobyInstancesEndAddr;

        public class WatchedAddress
        {
            public uint address;
            public int subID;
            public uint size;
            public bool isFloat;
            public bool isInstr;
            public bool hexRepresented;
            public bool isFrozen;
            public int freezeSub;
            public byte[] originalBytes;
            public string type;
            public string name;
        }

        public static void SetMobyInstancesAddress(uint instances, uint instancesEnd)
        {
            mobyInstancesAddr = instances;
            mobyInstancesEndAddr = instancesEnd;
        }

        private static readonly HashSet<string> MobyInspectorSupportedGames = new HashSet<string>
        {
            "NPEA00385", // RAC1
            "NPEA00386", // RAC2
            "NPEA00387", // RAC3
            "NPEA00423", // RAC4 (Deadlocked)
        };


        private const int CollapsedClientWidth = 428;
        private int expandedClientWidth;

        public MemoryForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            watchedMemoryAddressesListView.DoubleBuffering(true);
            mobyInspectorListView.DoubleBuffering(true);

            expandedClientWidth = this.ClientSize.Width;

            ApplyMobyInspectorVisibility();
        }

        private bool ShouldShowMobyInspector()
        {
            if (!MobyInspectorSupportedGames.Contains(AttachPS3Form.game))
                return false;

            string saved;
            try { saved = func.GetConfigData("config.txt", "mobyInspectorVisible"); }
            catch { saved = ""; }

            if (string.IsNullOrEmpty(saved)) return true;
            return !saved.Equals("false", StringComparison.OrdinalIgnoreCase);
        }

        private void ApplyMobyInspectorVisibility()
        {
            bool supported = MobyInspectorSupportedGames.Contains(AttachPS3Form.game);
            bool show = supported && ShouldShowMobyInspector();

            mobyInspectorLabel.Visible = show;
            selectedMobyComboBox.Visible = show;
            refreshMobysButton.Visible = show;
            dumpButton.Visible = show;
            sortByOClassCheckBox.Visible = show;
            mobyInspectorListView.Visible = show;

            this.ClientSize = new System.Drawing.Size(
                show ? expandedClientWidth : CollapsedClientWidth,
                this.ClientSize.Height);

            if (!supported)
            {
                toggleMobyInspectorButton.Enabled = false;
                toggleMobyInspectorButton.Text = "Unsupported for this game";
            }
            else
            {
                toggleMobyInspectorButton.Enabled = true;
                toggleMobyInspectorButton.Text = show ? "Hide moby viewer" : "Show moby viewer";
            }
        }

        private void toggleMobyInspectorButton_Click(object sender, EventArgs e)
        {
            if (!MobyInspectorSupportedGames.Contains(AttachPS3Form.game))
                return;

            bool currentlyShown = mobyInspectorListView.Visible;
            bool newShown = !currentlyShown;

            try
            {
                // Make sure config.txt exists before ChangeFileLines tries to read it.
                if (!File.Exists("config.txt"))
                    File.Create("config.txt").Close();

                func.ChangeFileLines("config.txt", newShown ? "true" : "false", "mobyInspectorVisible");
            }
            catch
            {
            }

            ApplyMobyInspectorVisibility();
        }

        IPS3API api = func.api;

        void SetItemValueText(ListViewItem item, string value, string frozen = "")
        {
            var watched = (WatchedAddress)item.Tag;
            if (watched.isFrozen) frozen = "❄: ";
            this.Invoke(new Action(() =>
            {
                item.SubItems[2].Text = frozen+value;
            }));
        }

        private static string FormatWatchedValue(WatchedAddress watched, byte[] bytes)
        {
            if (watched.isFloat)
            {
                return BitConverter.ToSingle(bytes, 0).ToString();
            }
            if (watched.isInstr) {
                using (var disassembler = CapstoneDisassembler.CreatePowerPcDisassembler(PowerPcDisassembleMode.BigEndian | PowerPcDisassembleMode.Bit64)) {
                    PowerPcInstruction[] instructions = disassembler.Disassemble(bytes.Reverse().ToArray(), (long)watched.address);

                    var insn = instructions[0];
                    return insn.Mnemonic + " " + insn.Operand;
                }
            }
            long val;

            switch (watched.size)
            {
                case 1:
                    val = bytes[0];
                    break;
                case 2:
                    val = BitConverter.ToInt16(bytes, 0);
                    break;
                case 4:
                    val = BitConverter.ToInt32(bytes, 0);
                    break;
                case 8:
                    val = BitConverter.ToInt64(bytes, 0);
                    break;
                default:
                    val = bytes[0];
                    break;
            }

            return watched.hexRepresented ? val.ToString("X") : val.ToString();
        }

        // New method for adding a memory watch entry
        private void AddMemoryWatch(uint address, string type, string name = "Unknown")
        {
            ListViewItem item = new ListViewItem(name);  // default to unknown if no name is supplied ^^^^^
            item.SubItems.Add(address.ToString("X")); 
            item.SubItems.Add("Waiting..."); 

            WatchedAddress watched = new WatchedAddress
            {
                address = address,
                type = type,
                name = name 
            };

            switch (watched.type)
            {
                case "Int32":
                    watched.size = 4;
                    break;
                case "Int64":
                    watched.size = 8;
                    break;
                case "Int16":
                    watched.size = 2;
                    break;
                case "Byte":
                    watched.size = 1;
                    break;
                case "Float":
                    watched.size = 4;
                    watched.isFloat = true;
                    break;
                case "Pointer":
                    watched.size = 4;
                    watched.hexRepresented = true;
                    break;
                case "Instruction":
                    watched.size = 4;
                    watched.isInstr = true;
                    break;
                default:
                    watched.size = 4;
                    watched.hexRepresented = true;
                    break;
            }


            item.Tag = watched;


            try
            {
                byte[] initial = api.ReadMemory(api.getCurrentPID(), address, watched.size);
                watched.originalBytes = initial;
                item.SubItems[2].Text = FormatWatchedValue(watched, initial);
            }
            catch (Exception)
            {
                
            }

            // Subscribe to the memory watch and get data updates
            watched.subID = api.SubMemory(api.getCurrentPID(), address, watched.size, (byte[] bytes) =>
            {
                SetItemValueText(item, FormatWatchedValue(watched, bytes));
            });

            watchedMemoryAddressesListView.Items.Add(item);
        }


        private void addMemoryWatchButton_Click(object sender, EventArgs e)
        {
            uint address = 0;

            try
            {
                // Parse address
                var strippedText = registerAddressTextBox.Text.Replace(" ", "").Replace("0x", "").Trim();
                address = Convert.ToUInt32(strippedText, 16);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Address must be hexadecimal");
                return;
            }

            AddMemoryWatch(address, registerAddressTypeCombo.Text);
        }


        private void MemoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (ListViewItem item in watchedMemoryAddressesListView.Items)
            {
                if (item.Tag != null)
                {
                    WatchedAddress watched = (WatchedAddress)item.Tag;
                    func.api.ReleaseSubID(watched.subID);
                    if (watched.isFrozen)
                        func.api.ReleaseSubID(watched.freezeSub);
                }
            }

            if(AttachPS3Form.notSupported)
            {
                Application.Exit();
            }
        }

        private void watchedMemoryAddressesListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var focusedItem = watchedMemoryAddressesListView.FocusedItem;
                if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                {
                    ContextMenuStrip menuStrip = new ContextMenuStrip();
                    menuStrip.Items.Add("Edit value...");
                    menuStrip.Items[0].Click += MenuStripEditValue_Click;

                    menuStrip.Items.Add("Rename...");
                    menuStrip.Items[1].Click += MenuStripRename_Click;

                    menuStrip.Items.Add("Freeze/unfreeze value");
                    menuStrip.Items[2].Click += MenuStripEditValue_Freeze;

                    menuStrip.Items.Add("Assemble");
                    menuStrip.Items[3].Click += MenuStripAssemble_Click;

                    menuStrip.Items.Add("Restore");
                    menuStrip.Items[4].Click += MenuStripRestore_Click;

                    menuStrip.Items.Add("Delete");
                    menuStrip.Items[5].Click += MenuStripDelete_Click;

                    menuStrip.Show(Cursor.Position);

                }
            }
        }
        private void MenuStripAssemble_Click(object sender, EventArgs e) {
            var watched = (WatchedAddress)watchedMemoryAddressesListView.FocusedItem?.Tag;

            if (watched == null || !watched.isInstr) 
                return;

            var dialog = new SimpleInputDialogForm("Assemble instruction", watchedMemoryAddressesListView.FocusedItem.SubItems[2].Text);

            if (dialog.ShowDialog() == DialogResult.OK) {
                try {
                    using (var engine = new Engine(Keystone.Architecture.PPC, Mode.PPC64 | Mode.BIG_ENDIAN)) {
                        var word = engine.Assemble(dialog.inputTextBox.Text, watched.address).Buffer;
                        func.api.WriteMemory(func.api.getCurrentPID(), watched.address, 4, word);
                    }
                }
                catch (Exception ex) { MessageBox.Show("Assembly failed: " + ex.Message); }
            }
        }

        private void MenuStripRestore_Click(object sender, EventArgs e) {
            var focusedItem = watchedMemoryAddressesListView.FocusedItem;
            if (focusedItem != null) {
                if (focusedItem.Tag != null) {
                    WatchedAddress watched = (WatchedAddress)focusedItem.Tag;

                    api.WriteMemory(api.getCurrentPID(), watched.address, watched.size, watched.originalBytes);
                }
            }
        }

        private void MenuStripEditValue_Freeze(object sender, EventArgs e)
        {
            var focusedItem = watchedMemoryAddressesListView.FocusedItem;
            if (focusedItem != null)
            {
                if (focusedItem.Tag != null)
                {
                    WatchedAddress watched = (WatchedAddress)focusedItem.Tag;
                    IPS3API api = func.api;
                    try
                    {
                        if (watched.isFrozen)
                        {
                            watched.isFrozen = false;
                            api.ReleaseSubID(watched.freezeSub);
                            watched.freezeSub = 0;
                        } 
                        else
                        {
                            watched.isFrozen = true;
                            // Get current value to freeze
                            byte[] currVal = api.ReadMemory(api.getCurrentPID(), watched.address, watched.size);
                            watched.freezeSub = api.FreezeMemory(api.getCurrentPID(), watched.address, watched.size, IPS3API.MemoryCondition.Any, currVal);

                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void MenuStripEditValue_Click(object sender, EventArgs e)
        {
            var focusedItem = watchedMemoryAddressesListView.FocusedItem;
            if (focusedItem != null)
            {
                if (focusedItem.Tag != null)
                {
                    WatchedAddress watched = (WatchedAddress)focusedItem.Tag;

                    IPS3API api = func.api;


                    string defaultText = focusedItem.SubItems[2].Text;
                    const string frozenPrefix = "❄: ";

                    if (defaultText.StartsWith(frozenPrefix))
                        defaultText = defaultText.Substring(frozenPrefix.Length);

                    if (watched.hexRepresented && !defaultText.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                        defaultText = "0x" + defaultText;

                    SimpleInputDialogForm inputDialog = new SimpleInputDialogForm("Edit value", defaultText);
                    if (inputDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            string text = inputDialog.inputTextBox.Text.Trim();
                            bool isHex = text.StartsWith("0x", StringComparison.OrdinalIgnoreCase);
                            string hexBody = isHex ? text.Substring(2) : null;

                            if (watched.isFloat)
                            {
                                float value;
                                if (isHex)
                                {
                                    uint bits = UInt32.Parse(hexBody, System.Globalization.NumberStyles.HexNumber);
                                    value = BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
                                }
                                else
                                {
                                    value = Convert.ToSingle(text);
                                }

                                api.WriteMemory(api.getCurrentPID(), watched.address, watched.size, BitConverter.GetBytes(value).Take((int)watched.size).Reverse().ToArray());
                            }
                            else
                            {
                                long value;
                                if (isHex)
                                {
                                    value = (long)UInt64.Parse(hexBody, System.Globalization.NumberStyles.HexNumber);
                                }
                                else
                                {
                                    value = Convert.ToInt64(text);
                                }

                                api.WriteMemory(api.getCurrentPID(), watched.address, watched.size, BitConverter.GetBytes(value).Take((int)watched.size).Reverse().ToArray());
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }

        private void MenuStripRename_Click(object sender, EventArgs e)
        {
            var focusedItem = watchedMemoryAddressesListView.FocusedItem;
            if (focusedItem == null || focusedItem.Tag == null) return;

            WatchedAddress watched = (WatchedAddress)focusedItem.Tag;

            SimpleInputDialogForm inputDialog = new SimpleInputDialogForm("Rename watch", watched.name);
            if (inputDialog.ShowDialog() == DialogResult.OK)
            {
                string newName = inputDialog.inputTextBox.Text;
                if (string.IsNullOrWhiteSpace(newName)) return;

                watched.name = newName;
                focusedItem.Text = newName;
            }
        }

        private void MenuStripDelete_Click(object sender, EventArgs e)
        {
            var focusedItem = watchedMemoryAddressesListView.FocusedItem;
            if (focusedItem != null)
            {
                if (focusedItem.Tag != null)
                {
                    WatchedAddress watched = (WatchedAddress)focusedItem.Tag;

                    IPS3API api = func.api;

                    api.ReleaseSubID(watched.subID);

                    watchedMemoryAddressesListView.Items.Remove(focusedItem);
                }
            }
        }


        private class MobyEntry
        {
            public uint address;
            public ushort oClass;
            public byte state;
            public string Display => $"0x{address:X}: 0x{oClass:X} ({oClass}) (state: {state})";
        }
        private List<MobyEntry> mobyEntries = new List<MobyEntry>();

        public void PopulateMobyComboBox()
        {
            var pid = func.api.getCurrentPID();

            uint instance = BitConverter.ToUInt32(func.api.ReadMemory(pid, mobyInstancesAddr, 4).Reverse().ToArray(), 0);
            uint end = BitConverter.ToUInt32(func.api.ReadMemory(pid, mobyInstancesEndAddr, 4).Reverse().ToArray(), 0);

            int offset_oClass, offset_state;

            if(AttachPS3Form.game == "NPEA00385")
            {
                // Dynamically get offsets from the RAC1.Moby struct
                offset_oClass = (int)Marshal.OffsetOf(typeof(rac1.Moby), nameof(rac1.Moby.oClass));
                offset_state = (int)Marshal.OffsetOf(typeof(rac1.Moby), nameof(rac1.Moby.state));
            }
            else if (AttachPS3Form.game == "NPEA00423")
            {
                // Dynamically get offsets from the RAC4.Moby struct
                offset_oClass = (int)Marshal.OffsetOf(typeof(rac4.Moby), nameof(rac4.Moby.oClass));
                offset_state = (int)Marshal.OffsetOf(typeof(rac4.Moby), nameof(rac4.Moby.state));
            }
            else
            {
                // Dynamically get offsets from the RAC2.Moby struct
                offset_oClass = (int)Marshal.OffsetOf(typeof(rac2.Moby), nameof(rac2.Moby.oClass));
                offset_state = (int)Marshal.OffsetOf(typeof(rac2.Moby), nameof(rac2.Moby.state));
            }

            mobyEntries.Clear();

            while (instance < end)
            {
                // Read oClass (2 bytes)
                ushort oClass = BitConverter.ToUInt16(
                    func.api.ReadMemory(pid, instance + (uint)offset_oClass, 2).Reverse().ToArray(), 0);

                // Read state (1 byte)
                byte state = func.api.ReadMemory(pid, instance + (uint)offset_state, 1)[0];

                mobyEntries.Add(new MobyEntry { address = instance, oClass = oClass, state = state });

                // Next Moby (each entry = 0x100 bytes)
                instance += 0x100;
            }

            RenderMobyComboBox();
        }

        // Render mobyEntries into the combo box, applying current sort toggle.
        // The combo's SelectedIndex corresponds to mobyEntries[i] after this call.
        private void RenderMobyComboBox()
        {
            IEnumerable<MobyEntry> ordered = mobyEntries;
            if (sortByOClassCheckBox.Checked)
                ordered = mobyEntries.OrderBy(m => m.oClass).ThenBy(m => m.address);

            // Reassign so indices line up with what we render
            mobyEntries = ordered.ToList();

            selectedMobyComboBox.Items.Clear();
            foreach (var entry in mobyEntries)
                selectedMobyComboBox.Items.Add(entry.Display);
        }

        private void sortByOClassCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (mobyEntries.Count == 0) return;
            RenderMobyComboBox();
        }
        public void PopulateMobyInspectorRac2(int index)
        {
            if (index < 0 || index >= mobyEntries.Count) return;

            var pid = func.api.getCurrentPID();
            uint mobyAddress = mobyEntries[index].address;

            byte[] memory = func.api.ReadMemory(pid, mobyAddress, 0x100);


            object moby;
            Type type;
            FieldInfo[] fields;

            // Pick which Moby type to use based on the game
            if (AttachPS3Form.game == "NPEA00385")
            {
                moby = rac1.Moby.ByteArrayToMoby(memory);
                type = typeof(rac1.Moby);
                fields = type.GetFields();
            }
            else if (AttachPS3Form.game == "NPEA00423")
            {
                moby = rac4.Moby.ByteArrayToMoby(memory);
                type = typeof(rac4.Moby);
                fields = type.GetFields();
            }
            else
            {
                moby = rac2.Moby.ByteArrayToMoby(memory);
                type = typeof(rac2.Moby);
                fields = type.GetFields();
            }

            while (mobyInspectorListView.Items.Count < fields.Length)
                mobyInspectorListView.Items.Add(new ListViewItem(new string[3]));

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var offset = Marshal.OffsetOf(type, field.Name).ToInt32();

                var item = mobyInspectorListView.Items[i];
                item.Text = $"0x{(mobyAddress + (uint)offset):X}";
                item.SubItems[1].Text = field.Name;

                object value = field.GetValue(moby);
                string display = "";

                if (field.FieldType == typeof(rac2.Vec4))
                {
                    var vec = (rac2.Vec4)value;
                    display = $"x: {vec.x}, y: {vec.y}, z: {vec.z}, w: {vec.w}";
                }
                else if (field.FieldType == typeof(rac1.Vec4))
                {
                    var vec = (rac1.Vec4)value;
                    display = $"x: {vec.x}, y: {vec.y}, z: {vec.z}, w: {vec.w}";
                }
                else if (field.FieldType == typeof(rac4.Vec4))
                {
                    var vec = (rac4.Vec4)value;
                    display = $"x: {vec.x}, y: {vec.y}, z: {vec.z}, w: {vec.w}";
                }
                else if (field.FieldType == typeof(rac2.GamePtr))
                {
                    display = $"0x{((rac2.GamePtr)value).addr:X}";
                }
                else if (field.FieldType == typeof(rac1.GamePtr))
                {
                    display = $"0x{((rac1.GamePtr)value).addr:X}";
                }
                else if (field.FieldType == typeof(rac4.GamePtr))
                {
                    display = $"0x{((rac4.GamePtr)value).addr:X}";
                }
                else if (field.FieldType == typeof(sbyte))
                {
                    display = $"0x{(((sbyte)value) & 0xFF):X}";
                }

                else if (field.FieldType == typeof(uint) ||
                         field.FieldType == typeof(ushort) ||
                         field.FieldType == typeof(byte))
                {
                    display = $"0x{Convert.ToUInt64(value):X}";
                }
                else if (field.FieldType == typeof(int) ||
                         field.FieldType == typeof(short))
                {
                    long signedValue = Convert.ToInt64(value);
                    display = $"0x{(signedValue & 0xFFFFFFFF):X}";
                }

                else
                {
                    display = value?.ToString() ?? "null";
                }

                item.SubItems[2].Text = display;
            }
        }

        private void refreshMobysButton_Click(object sender, EventArgs e)
        {
            if (AttachPS3Form.game == "NPEA00386" || AttachPS3Form.game == "NPEA00385" || AttachPS3Form.game == "NPEA00387" || AttachPS3Form.game == "NPEA00423")
            {
                WebMAN wmm = new WebMAN(func.api.GetIP());
                wmm.PauseRSX();
                   
                try
                {
                    PopulateMobyComboBox();
                } catch (Exception ex)
                {
                    wmm.ContinueRSX();
                    throw ex;
                }
                wmm.ContinueRSX();
            } 
            else
            {
                MessageBox.Show("Game is not supported.");
                Console.WriteLine(AttachPS3Form.game);
            }
        }
        System.Timers.Timer timer = new System.Timers.Timer(1000); // Set up the timer for 1 second intervals (1000 milliseconds = 1 second)

        private void selectedMobyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AttachPS3Form.game == "NPEA00386" || AttachPS3Form.game == "NPEA00387" || AttachPS3Form.game == "NPEA00385" || AttachPS3Form.game == "NPEA00423")
            {
                timer.Elapsed += (s, evt) =>
                {
                    // Inline function (or lambda expression) to be executed every second
                    this.Invoke(new Action(() =>
                    {
                        PopulateMobyInspectorRac2(((ComboBox)sender).SelectedIndex);
                    }));
                };
                timer.Start(); // Start the timer

                PopulateMobyInspectorRac2(((ComboBox)sender).SelectedIndex);
            }
        }

        private void MemoryForm_Load(object sender, EventArgs e)
        {
            UpdateWatchlists();
        }

        private void UpdateWatchlists()
        {
            var watchlistItems = savedWatchlistsComboBox.Items;
            string gamePrefix = $"{AttachPS3Form.game}-"; 

            if (!Directory.Exists("watchlists"))
                return;

            watchlistItems.Clear(); // clear existing items

            foreach (string filePath in Directory.GetFiles("watchlists", $"{gamePrefix}*.mw")) // Only get files for the current game
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                // remove game ID prefix
                if (fileName.StartsWith(gamePrefix))
                    fileName = fileName.Substring(gamePrefix.Length);

                watchlistItems.Add(fileName);
            }
        }

        private void SaveWatchListToFile(string filename)
        {
            // Prepare the data to write (address, type, and name from the ListViewItem)
            var watchedAddressesData = watchedMemoryAddressesListView.Items
                .Cast<ListViewItem>() 
                .Select(item =>
                {
                    var watched = (WatchedAddress)item.Tag;
                    string name = item.Text; 
                    return $"{name},0x{watched.address.ToString("X")},{watched.type}";  
                })
                .ToList();

            try
            {
                File.WriteAllLines(filename, watchedAddressesData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show(
                    "Unable to create or write to file. This may be caused by an invalid name.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        // new method to populate the listview from file
        private void PopulateWatchlistFromFile(string filename)
        {
            try
            {
                var lines = File.ReadAllLines(filename);

                foreach (var line in lines)
                {
                    var parts = line.Split(',');

                    if (parts.Length == 3) 
                    {
                        string name = parts[0];
                        uint address = Convert.ToUInt32(parts[1], 16);
                        string type = parts[2];
                        AddMemoryWatch(address, type, name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading watchlist file: " + ex.Message);
            }
        }



        private void saveWatchListButton_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory("watchlists");
            string filename = $"watchlists/{AttachPS3Form.game}-{savedWatchlistsComboBox.Text}.mw";

            try
            {
                SaveWatchListToFile(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                MessageBox.Show(
                    "Unable to create or write to file. This may be caused by an invalid name.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            UpdateWatchlists();
        }

        private void savedWatchlistsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // release sub ids before clearing the list view
            foreach (ListViewItem item in watchedMemoryAddressesListView.Items)
            {
                if (item.Tag != null)
                {
                    WatchedAddress watched = (WatchedAddress)item.Tag;
                    if (watched.subID != 0)
                    {
                        func.api.ReleaseSubID(watched.subID);
                    }
                    if (watched.isFrozen)
                        func.api.ReleaseSubID(watched.freezeSub);
                }
            }

            // clear the list view and populate with the new stuuuufffffff aww ye
            watchedMemoryAddressesListView.Items.Clear();

            string filename = $"watchlists/{AttachPS3Form.game}-{savedWatchlistsComboBox.Text}.mw";
            PopulateWatchlistFromFile(filename);
        }

        private void dumpButton_Click(object sender, EventArgs e)
        {
            if (selectedMobyComboBox.Items.Count == 0)
            {
                MessageBox.Show("Please refresh the moby list.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog { FileName = $"moby dump.txt" };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int i = 0; i < selectedMobyComboBox.Items.Count; i++)
                            writer.WriteLine($"Moby #{i}: {selectedMobyComboBox.Items[i]}");
                    }
                }
                catch{ }
            }
        }
    }
}
