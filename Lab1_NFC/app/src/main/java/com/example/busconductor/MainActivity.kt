package com.example.busconductor

import android.app.PendingIntent
import android.content.Context
import android.content.IntentFilter
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.view.View
import android.content.Intent
import android.nfc.*
import android.widget.*
import android.nfc.NfcAdapter
import android.nfc.tech.MifareClassic
import android.os.CountDownTimer

import java.lang.Exception
import java.io.IOException
import java.util.*

class MainActivity : AppCompatActivity() {

    var nfcAdapter: NfcAdapter? = null
    var pendingIntent:PendingIntent? = null
    var intentFilter = arrayOf<IntentFilter>()
    var tag:Tag? = null
    var NFCContent: TextView? = null
    var defaultValueNFCText = ""
    var spinner:Spinner? = null
    var credits:TextView? = null
    var addCredits:Button? = null
    var dailyTicket:Button? = null
    var oneHourTicket:Button? = null
    var checkTicket:Button? = null
    var creditText:EditText? = null
    var deleteTickets:ImageButton? = null

    var shouldUpdateCredits = false
    var shouldBuyDailyTicket = false
    var shouldBuyOneHourTicket = false
    var shouldCheckTicketTime = false
    var shouldDeleteTickets = false
    var timer:CountDownTimer = object: CountDownTimer(5000, 1000) {
        override fun onTick(millisUntilFinished: Long) {}

        override fun onFinish() {NFCContent!!.text = "Pridėkite miesto kortelę, kad nuskaityti duomenis"}
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)
        //context = this;

        NFCContent = findViewById(R.id.nfc_contents) as TextView
        spinner = findViewById(R.id.spinner) as Spinner
        credits = findViewById(R.id.text_view) as TextView
        addCredits = findViewById(R.id.button5) as Button
        dailyTicket = findViewById(R.id.button2) as Button
        oneHourTicket = findViewById(R.id.button3) as Button
        checkTicket = findViewById(R.id.button4) as Button
        creditText = findViewById(R.id.editTextCreditCount) as EditText
        defaultValueNFCText = NFCContent!!.text.toString()
        deleteTickets = findViewById(R.id.deleteButton) as ImageButton

        addCredits!!.setOnClickListener(object : View.OnClickListener {
            override fun onClick(v: View?) {
                shouldUpdateCredits = true
                shouldBuyDailyTicket = false
                shouldBuyOneHourTicket = false
                shouldCheckTicketTime = false
                NFCContent!!.text = defaultValueNFCText
                timer.cancel();
            }
        })

        dailyTicket!!.setOnClickListener(object : View.OnClickListener {
            override fun onClick(v: View?) {
                shouldUpdateCredits = false
                shouldBuyDailyTicket = true
                shouldBuyOneHourTicket = false
                shouldCheckTicketTime = false
                NFCContent!!.text = defaultValueNFCText
                timer.cancel();
            }
        })

        oneHourTicket!!.setOnClickListener(object : View.OnClickListener {
            override fun onClick(v: View?) {
                shouldUpdateCredits = false
                shouldBuyDailyTicket = false
                shouldBuyOneHourTicket = true
                shouldCheckTicketTime = false
                NFCContent!!.text = defaultValueNFCText
                timer.cancel();
            }
        })

        checkTicket!!.setOnClickListener(object : View.OnClickListener {
            override fun onClick(v: View?) {
                shouldUpdateCredits = false
                shouldBuyDailyTicket = false
                shouldBuyOneHourTicket = false
                shouldCheckTicketTime = true
                NFCContent!!.text = defaultValueNFCText
                timer.cancel();
            }
        })

        deleteTickets!!.setOnClickListener(object : View.OnClickListener {
            override fun onClick(v: View?) {
                shouldUpdateCredits = false
                shouldBuyDailyTicket = false
                shouldBuyOneHourTicket = false
                shouldCheckTicketTime = false
                shouldDeleteTickets = true
                NFCContent!!.text = defaultValueNFCText
                timer.cancel();
            }
        })

        initNfcAdapter()

        if (nfcAdapter == null) {
            // Stop here, we definitely need NFC
            Toast.makeText(this, "Šitas prietaisas nepalaiko NFC funkcionalumo", Toast.LENGTH_LONG).show()
            finish()
        }
        readFromIntent(intent)

        pendingIntent = PendingIntent.getActivity(
            this,
            0,
            Intent(this, javaClass).addFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP),
            PendingIntent.FLAG_MUTABLE
        )
        val tagDetected = IntentFilter(NfcAdapter.ACTION_TAG_DISCOVERED)

        tagDetected.addCategory(Intent.CATEGORY_DEFAULT)
        intentFilter = arrayOf<IntentFilter>(tagDetected)
    }

    private fun initNfcAdapter() {
        val nfcManager = getSystemService(Context.NFC_SERVICE) as NfcManager
        nfcAdapter = nfcManager.defaultAdapter
    }

    private fun readFromIntent(intent: Intent) {
        val action = intent.action
        if (NfcAdapter.ACTION_TAG_DISCOVERED == action || NfcAdapter.ACTION_TECH_DISCOVERED == action || NfcAdapter.ACTION_NDEF_DISCOVERED == action) {
            val raMsgs:Tag? = intent.getParcelableExtra(NfcAdapter.EXTRA_TAG) as Tag?

            try {
                val mfc = MifareClassic.get(raMsgs)
                mfc.connect()
                cardData(mfc)

                if (shouldUpdateCredits) {
                    shouldUpdateCredits = false
                    addCredits(mfc, creditText!!.text.toString().toInt())
                    NFCContent!!.text = defaultValueNFCText
                }

                if (shouldBuyDailyTicket) {
                    shouldBuyDailyTicket = false
                    BuyTicket(mfc, 1, true,10)
                    timer.start()
                }

                if (shouldBuyOneHourTicket) {
                    shouldBuyOneHourTicket = false
                    BuyTicket(mfc, 2, false,1)
                    timer.start()
                }

                if (shouldCheckTicketTime) {
                    shouldCheckTicketTime = false
                    var result = checkSelectedCity(mfc)
                    if (result) {
                        NFCContent!!.text = "Bilietas galioja"
                    } else {
                        NFCContent!!.text = "Galiojančio bilieto nėra"
                    }
                    timer.start()
                }

                if (shouldDeleteTickets) {
                    shouldDeleteTickets = false
                    DeleteTickets(mfc)
                    cardData(mfc)
                    timer.start()
                }
            } catch (e:Exception) {
                Toast.makeText(this, "Dėmėsio! Pridėkite kortelę atgal!", Toast.LENGTH_SHORT).show();
            }

        }
    }

    fun BuyTicket(mfc: MifareClassic, sector: Int, dailyTicket:Boolean, price:Int)
    {
        val credits = readCredits(mfc)
        var authA = mfc.authenticateSectorWithKeyA(sector, MifareClassic.KEY_DEFAULT)
        if (authA) {
            var first = readDataFromBlock(mfc, sector, spinner!!.selectedItemId.toInt())
            var data: Long = bytesToLong(first, 0)

            if (credits >= price && Date().time >= data) {
                var dt = Date()
                val c = Calendar.getInstance()
                c.time = dt
                if(dailyTicket)
                {
                    c.add(Calendar.HOUR, 24)
                }
                else{
                    c.add(Calendar.HOUR, 1)
                }
                dt = c.time
                var byteRes: ByteArray = ByteArray(16)
                longToBytes(byteRes, dt.time, 0)
                writeData(mfc, sector, spinner!!.selectedItemId.toInt(), byteRes)
                decreaseCredits(mfc, price)
                NFCContent!!.text = "Bilietas nupirktas"
            } else if (Date().time < data) {
                if(credits >= price) {
                    NFCContent!!.text = "Bilietas vis dar galioja"
                }
                else
                {
                    NFCContent!!.text = "Kortelėje trūksta pinigų nupirkti bilietui,\n tačiau tavo bilietas vis dar galioja"
                }
            } else {
                NFCContent!!.text = "Kortelėje trūksta pinigų nupirkti bilietui"
            }
        } else {
            NFCContent!!.text = "Problema su kortele, patikrinti ar kortelė nėra pažeista"
        }

    }

    private fun cardData(mfc: MifareClassic)
    {
        NFCContent!!.text = ""

        for (i in 1..2){
            var text:String = NFCContent!!.text.toString()
            if(i == 1)
            {
                NFCContent!!.text = text + " Dienos biletai: \n"
            }
            else
            {
                NFCContent!!.text = text + " Valandos biletai: \n"
            }
            var authA = mfc.authenticateSectorWithKeyA(i, MifareClassic.KEY_DEFAULT)
            if(authA) {
                var blockData = readDataFromBlock(mfc, i, 0)
                var dateInMiliseconds: Long = bytesToLong(blockData, 0)

                if(isDateValid(dateInMiliseconds)) {
                    text = NFCContent!!.text.toString()
                    NFCContent!!.text =
                        text + "Kauno bilieto galiojimo laikas: " + Date(dateInMiliseconds).toString() + "\n"
                }

                blockData = readDataFromBlock(mfc, i, 1)
                dateInMiliseconds = bytesToLong(blockData, 0)
                if(isDateValid(dateInMiliseconds)) {
                    text = NFCContent!!.text.toString()
                    NFCContent!!.text =
                        text + "Klaipedos bilieto galiojimo laikas: " + Date(dateInMiliseconds).toString() + "\n"
                }

                blockData = readDataFromBlock(mfc, i, 2)
                dateInMiliseconds = bytesToLong(blockData, 0)
                if(isDateValid(dateInMiliseconds)) {
                    text = NFCContent!!.text.toString()
                    NFCContent!!.text =
                        text + "Vilniaus bilieto galiojimo laikas: " + Date(dateInMiliseconds).toString() + "\n"
                }
            }
            readCredits(mfc)
        }
    }

    private fun isDateValid(data: Long): Boolean {
        var dtBefore = Date()
        var dtAfter = Date()
        val a = Calendar.getInstance()
        a.time = dtAfter
        val b = Calendar.getInstance()
        b.time = dtBefore
        a.add(Calendar.YEAR, -10);
        b.add(Calendar.YEAR, 2)
        dtBefore = a.time
        dtAfter = b.time

        return data.compareTo(dtBefore.time) >= 0 && data.compareTo(dtAfter.time) == -1
    }

    private fun readCredits(mfc:MifareClassic):Int
    {
        var authA = mfc.authenticateSectorWithKeyA(3, MifareClassic.KEY_DEFAULT)

        if(authA) {
            var bytes = readDataFromBlock(mfc, 3, 0)
            var intRes = bytesToInt(bytes, 0)
            credits?.text = "Kortelės likutis (eur): " + intRes
            return intRes
        }
        return  0
    }

    private fun addCredits(mfc:MifareClassic, creditsToAdd:Int)
    {
        var authA = mfc.authenticateSectorWithKeyA(3, MifareClassic.KEY_DEFAULT)

        if(authA) {
            var bytes = readDataFromBlock(mfc, 3, 0)
            var intRes = bytesToInt(bytes, 0)
            intRes += creditsToAdd
            var byteRes: ByteArray = ByteArray(16)
            intToBytes(byteRes, intRes, 0)
            writeData(mfc, 3, 0, byteRes)
            credits?.text = "Kortelės likutis (eur): " + intRes
        }
    }

    private fun decreaseCredits(mfc:MifareClassic, creditsToDecrease:Int)
    {
        var authA = mfc.authenticateSectorWithKeyA(3, MifareClassic.KEY_DEFAULT)

        if(authA) {
            var bytes = readDataFromBlock(mfc, 3, 0)
            var intRes = bytesToInt(bytes, 0)
            intRes -= creditsToDecrease
            if (intRes < 0) {
                intRes = 0
            }
            var byteRes: ByteArray = ByteArray(16)
            intToBytes(byteRes, intRes, 0)
            writeData(mfc, 3, 0, byteRes)
            credits?.text = "Kortelės likutis (eur): " + intRes
        }
    }

    fun DeleteTickets(mfc: MifareClassic)
    {
           var emptyArray: ByteArray = ByteArray(16)

            for (sectorId in 0..3){
                var authA = mfc.authenticateSectorWithKeyA(1, MifareClassic.KEY_DEFAULT)
                if (authA) {
                    for (blockId in 0..2) {
                        writeData(mfc, sectorId, blockId, emptyArray)
                    }
                }
                else {
                    NFCContent!!.text = "Problema su kortele, patikrinti ar kortelė galioja ir nėra pažeista"
                }
            }
    }

    private fun checkSelectedCity(mfc:MifareClassic):Boolean
    {
        var authA = mfc.authenticateSectorWithKeyA(1, MifareClassic.KEY_DEFAULT)
        var authb = mfc.authenticateSectorWithKeyA(2, MifareClassic.KEY_DEFAULT)
        if(authA && authb) {
            var bytesDaily = readDataFromBlock(mfc, 1, spinner!!.selectedItemId.toInt())
            var bytesOneTime = readDataFromBlock(mfc, 2, spinner!!.selectedItemId.toInt())

            var longOneTime = bytesToLong(bytesOneTime, 0)
            var longDaily = bytesToLong(bytesDaily, 0)

            if (Date().time <= longDaily || Date().time <= longOneTime) {
                return true;
            }
        }
        return false
    }

    fun longToBytes(buffer: ByteArray, lng: Long, offset: Int) {

        buffer[offset + 0] = (lng shr 0).toByte()
        buffer[offset + 1] = (lng shr 8).toByte()
        buffer[offset + 2] = (lng shr 16).toByte()
        buffer[offset + 3] = (lng shr 24).toByte()
        buffer[offset + 4] = (lng shr 32).toByte()
        buffer[offset + 5] = (lng shr 40).toByte()
        buffer[offset + 6] = (lng shr 48).toByte()
        buffer[offset + 7] = (lng shr 56).toByte()

    }

    fun bytesToLong(bytes: ByteArray, offset: Int): Long {
        return (
                (bytes[offset + 7].toLong() shl 56) or
                (bytes[offset + 6].toLong() and 0xff shl 48) or
                (bytes[offset + 5].toLong() and 0xff shl 40) or
                (bytes[offset + 4].toLong() and 0xff shl 32) or
                (bytes[offset + 3].toLong() and 0xff shl 24) or
                (bytes[offset + 2].toLong() and 0xff shl 16) or
                (bytes[offset + 1].toLong() and 0xff shl 8) or
                (bytes[offset + 0].toLong() and 0xff)
                )
    }

    private fun intToBytes(buffer: ByteArray, data: Int, offset: Int) {
        buffer[offset + 0] = (data shr 0).toByte()
        buffer[offset + 1] = (data shr 8).toByte()
        buffer[offset + 2] = (data shr 16).toByte()
        buffer[offset + 3] = (data shr 24).toByte()
    }

    private fun bytesToInt(buffer: ByteArray, offset: Int): Int {
        return (buffer[offset + 3].toInt() shl 24) or
                (buffer[offset + 2].toInt() and 0xff shl 16) or
                (buffer[offset + 1].toInt() and 0xff shl 8) or
                (buffer[offset + 0].toInt() and 0xff)
    }

    private fun writeData(mfc:MifareClassic, sector:Int, block:Int, bytes: ByteArray){
        var bIndex = 0
        var authA = mfc.authenticateSectorWithKeyA(sector, MifareClassic.KEY_DEFAULT)
        if(authA) {
            bIndex = mfc.sectorToBlock(sector);
            var realIndex = bIndex + block
            mfc.writeBlock(realIndex, bytes)
        }
    }

    private fun readDataFromBlock(mfc:MifareClassic, sector: Int, blockIndex:Int):ByteArray {

        var data: ByteArray = ByteArray(1){0};
        var realBlockIndex = mfc.sectorToBlock(sector) + blockIndex
        var authA = mfc.authenticateSectorWithKeyA(sector, MifareClassic.KEY_DEFAULT)
        if(authA) {

            // 6.3) Read the block
            try {
                data = mfc.readBlock(realBlockIndex);
                // 7) Convert the data into a string from Hex format.

            }catch (ioe:IOException) {
                ioe.printStackTrace();
            } catch (e:Exception) {
                e.printStackTrace()
            }

        }
        return data;

    }

    override fun onNewIntent(intent: Intent) {
        setIntent(intent)
        readFromIntent(intent)
        if (NfcAdapter.ACTION_TAG_DISCOVERED == intent.action) {
            tag = intent.getParcelableExtra(NfcAdapter.EXTRA_TAG)
        }
        super.onNewIntent(intent)
    }

    override fun onResume() {
        super.onResume()
        val intent = Intent(this.applicationContext, this.javaClass)
        val pendingIntent = PendingIntent.getActivity(this.applicationContext, 0, intent,
            PendingIntent.FLAG_MUTABLE)
        nfcAdapter!!.enableForegroundDispatch(this, pendingIntent, null, null)
    }

    override fun onPause() {
        nfcAdapter!!.disableForegroundDispatch(this)
        super.onPause()
    }

}