<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method='text'/>
  <xsl:template match="/">
    <xsl:apply-templates/>
  </xsl:template>

  <xsl:variable name="newline">
    <xsl:text>&#xD;&#xA;</xsl:text>
  </xsl:variable>

  <xsl:variable name="break">
    <xsl:text>&#xD;&#xA;&#xD;&#xA;</xsl:text>
  </xsl:variable>

  <xsl:template match="test-run">
    <xsl:value-of select="concat(' Total: ', @total, ', Passed: ', @passed, ', Failed: ', @failed, ', Inconclusive: ', @inconclusive, ', Skipped: ', @skipped, ' in ', format-number(@duration,'0.000'), ' seconds', $newline)"/>

  </xsl:template>

</xsl:stylesheet>
