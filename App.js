// App.js
import React from 'react';
import { Text, SafeAreaView, StyleSheet } from 'react-native';
import { Card } from 'react-native-paper';
import AssetExample from './components/AssetExample';

export default function App() {
  return (
    <SafeAreaView style={styles.container}>
      <Text style={styles.paragraph}>
        {/* Optional helper text at top; you can remove if you like */}
        My BioSketch @ UC (built with React Native + Expo Snack)
      </Text>

      <Card style={styles.card}>
        <AssetExample />
      </Card>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    backgroundColor: '#e60026',   // assignment: preferred background color
    padding: 12,
  },
  paragraph: {
    margin: 12,
    fontSize: 16,
    fontWeight: '600',
    textAlign: 'center',
    color: 'white',
  },
  card: {
    borderRadius: 12,
    overflow: 'hidden',
    elevation: 3,
  },
});
