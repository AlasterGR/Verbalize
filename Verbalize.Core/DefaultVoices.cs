namespace Verbalize.Core
{
    /// <summary> A built-in list of voices, used whenever the full list cannot be downloaded from Azure. </summary>
    public static class DefaultVoices
    {
        /// <summary> The built-in voice list, in the same XML layout as the list downloaded from Azure. </summary>
        public const string Xml = @"<Voices>  
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (el-GR, AthinaNeural)</Name>
                                    <DisplayName>Athina</DisplayName>
                                    <LocalName>Αθηνά</LocalName>
                                    <ShortName>el-GR-AthinaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>el-GR</Locale>
                                    <LocaleName>Greek (Greece)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (el-GR, NestorasNeural)</Name>
                                    <DisplayName>Nestoras</DisplayName>
                                    <LocalName>Νέστορας</LocalName>
                                    <ShortName>el-GR-NestorasNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>el-GR</Locale>
                                    <LocaleName>Greek (Greece)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JennyNeural)</Name>
                                    <DisplayName>Jenny</DisplayName>
                                    <LocalName>Jenny</LocalName>
                                    <ShortName>en-US-JennyNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>assistant</StyleList>
                                    <StyleList>chat</StyleList>
                                    <StyleList>customerservice</StyleList>
                                    <StyleList>newscast</StyleList>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>152</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JennyMultilingualNeural)</Name>
                                    <DisplayName>Jenny Multilingual</DisplayName>
                                    <LocalName>Jenny Multilingual</LocalName>
                                    <ShortName>en-US-JennyMultilingualNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SecondaryLocaleList>de-DE</SecondaryLocaleList>
                                    <SecondaryLocaleList>en-AU</SecondaryLocaleList>
                                    <SecondaryLocaleList>en-CA</SecondaryLocaleList>
                                    <SecondaryLocaleList>en-GB</SecondaryLocaleList>
                                    <SecondaryLocaleList>es-ES</SecondaryLocaleList>
                                    <SecondaryLocaleList>es-MX</SecondaryLocaleList>
                                    <SecondaryLocaleList>fr-CA</SecondaryLocaleList>
                                    <SecondaryLocaleList>fr-FR</SecondaryLocaleList>
                                    <SecondaryLocaleList>it-IT</SecondaryLocaleList>
                                    <SecondaryLocaleList>ja-JP</SecondaryLocaleList>
                                    <SecondaryLocaleList>ko-KR</SecondaryLocaleList>
                                    <SecondaryLocaleList>pt-BR</SecondaryLocaleList>
                                    <SecondaryLocaleList>zh-CN</SecondaryLocaleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>190</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, GuyNeural)</Name>
                                    <DisplayName>Guy</DisplayName>
                                    <LocalName>Guy</LocalName>
                                    <ShortName>en-US-GuyNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>newscast</StyleList>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>215</WordsPerMinute>
                                  </Voice>
                                <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AmberNeural)</Name>
                                    <DisplayName>Amber</DisplayName>
                                    <LocalName>Amber</LocalName>
                                    <ShortName>en-US-AmberNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>152</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AnaNeural)</Name>
                                    <DisplayName>Ana</DisplayName>
                                    <LocalName>Ana</LocalName>
                                    <ShortName>en-US-AnaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>135</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AriaNeural)</Name>
                                    <DisplayName>Aria</DisplayName>
                                    <LocalName>Aria</LocalName>
                                    <ShortName>en-US-AriaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>chat</StyleList>
                                    <StyleList>customerservice</StyleList>
                                    <StyleList>narration-professional</StyleList>
                                    <StyleList>newscast-casual</StyleList>
                                    <StyleList>newscast-formal</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>empathetic</StyleList>
                                    <StyleList>angry</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>150</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AshleyNeural)</Name>
                                    <DisplayName>Ashley</DisplayName>
                                    <LocalName>Ashley</LocalName>
                                    <ShortName>en-US-AshleyNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>149</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, BrandonNeural)</Name>
                                    <DisplayName>Brandon</DisplayName>
                                    <LocalName>Brandon</LocalName>
                                    <ShortName>en-US-BrandonNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>156</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, ChristopherNeural)</Name>
                                    <DisplayName>Christopher</DisplayName>
                                    <LocalName>Christopher</LocalName>
                                    <ShortName>en-US-ChristopherNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>149</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, CoraNeural)</Name>
                                    <DisplayName>Cora</DisplayName>
                                    <LocalName>Cora</LocalName>
                                    <ShortName>en-US-CoraNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>146</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, DavisNeural)</Name>
                                    <DisplayName>Davis</DisplayName>
                                    <LocalName>Davis</LocalName>
                                    <ShortName>en-US-DavisNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>chat</StyleList>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, ElizabethNeural)</Name>
                                    <DisplayName>Elizabeth</DisplayName>
                                    <LocalName>Elizabeth</LocalName>
                                    <ShortName>en-US-ElizabethNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>152</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, EricNeural)</Name>
                                    <DisplayName>Eric</DisplayName>
                                    <LocalName>Eric</LocalName>
                                    <ShortName>en-US-EricNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>147</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JacobNeural)</Name>
                                    <DisplayName>Jacob</DisplayName>
                                    <LocalName>Jacob</LocalName>
                                    <ShortName>en-US-JacobNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JaneNeural)</Name>
                                    <DisplayName>Jane</DisplayName>
                                    <LocalName>Jane</LocalName>
                                    <ShortName>en-US-JaneNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, JasonNeural)</Name>
                                    <DisplayName>Jason</DisplayName>
                                    <LocalName>Jason</LocalName>
                                    <ShortName>en-US-JasonNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>156</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, MichelleNeural)</Name>
                                    <DisplayName>Michelle</DisplayName>
                                    <LocalName>Michelle</LocalName>
                                    <ShortName>en-US-MichelleNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, MonicaNeural)</Name>
                                    <DisplayName>Monica</DisplayName>
                                    <LocalName>Monica</LocalName>
                                    <ShortName>en-US-MonicaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>145</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, NancyNeural)</Name>
                                    <DisplayName>Nancy</DisplayName>
                                    <LocalName>Nancy</LocalName>
                                    <ShortName>en-US-NancyNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>149</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, SaraNeural)</Name>
                                    <DisplayName>Sara</DisplayName>
                                    <LocalName>Sara</LocalName>
                                    <ShortName>en-US-SaraNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>157</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, SteffanNeural)</Name>
                                    <DisplayName>Steffan</DisplayName>
                                    <LocalName>Steffan</LocalName>
                                    <ShortName>en-US-SteffanNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>154</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, TonyNeural)</Name>
                                    <DisplayName>Tony</DisplayName>
                                    <LocalName>Tony</LocalName>
                                    <ShortName>en-US-TonyNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <StyleList>angry</StyleList>
                                    <StyleList>cheerful</StyleList>
                                    <StyleList>excited</StyleList>
                                    <StyleList>friendly</StyleList>
                                    <StyleList>hopeful</StyleList>
                                    <StyleList>sad</StyleList>
                                    <StyleList>shouting</StyleList>
                                    <StyleList>terrified</StyleList>
                                    <StyleList>unfriendly</StyleList>
                                    <StyleList>whispering</StyleList>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>GA</Status>
                                    <WordsPerMinute>156</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AIGenerate1Neural)</Name>
                                    <DisplayName>AIGenerate1</DisplayName>
                                    <LocalName>AIGenerate1</LocalName>
                                    <ShortName>en-US-AIGenerate1Neural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>Preview</Status>
                                    <WordsPerMinute>135</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, AIGenerate2Neural)</Name>
                                    <DisplayName>AIGenerate2</DisplayName>
                                    <LocalName>AIGenerate2</LocalName>
                                    <ShortName>en-US-AIGenerate2Neural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>Preview</Status>
                                    <WordsPerMinute>140</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (en-US, RogerNeural)</Name>
                                    <DisplayName>Roger</DisplayName>
                                    <LocalName>Roger</LocalName>
                                    <ShortName>en-US-RogerNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>en-US</Locale>
                                    <LocaleName>English (United States)</LocaleName>
                                    <SampleRateHertz>48000</SampleRateHertz>
                                    <VoiceType>Neural</VoiceType>
                                    <Status>Preview</Status>
                                    <WordsPerMinute>143</WordsPerMinute>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sk-SK, LukasNeural)</Name>
                                    <DisplayName>Lukas</DisplayName>
                                    <LocalName>Lukáš</LocalName>
                                    <ShortName>sk-SK-LukasNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>sk-SK</Locale>
                                    <LocaleName>Slovak (Slovakia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sk-SK, ViktoriaNeural)</Name>
                                    <DisplayName>Viktoria</DisplayName>
                                    <LocalName>Viktória</LocalName>
                                    <ShortName>sk-SK-ViktoriaNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>sk-SK</Locale>
                                    <LocaleName>Slovak (Slovakia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sl-SI, PetraNeural)</Name>
                                    <DisplayName>Petra</DisplayName>
                                    <LocalName>Petra</LocalName>
                                    <ShortName>sl-SI-PetraNeural</ShortName>
                                    <Gender>Female</Gender>
                                    <Locale>sl-SI</Locale>
                                    <LocaleName>Slovenian (Slovenia)</LocaleName>
                                  </Voice>
                                  <Voice>
                                    <Name>Microsoft Server Speech Text to Speech Voice (sl-SI, RokNeural)</Name>
                                    <DisplayName>Rok</DisplayName>
                                    <LocalName>Rok</LocalName>
                                    <ShortName>sl-SI-RokNeural</ShortName>
                                    <Gender>Male</Gender>
                                    <Locale>sl-SI</Locale>
                                    <LocaleName>Slovenian (Slovenia)</LocaleName>
                                  </Voice>
                                </Voices>";
    }
}
