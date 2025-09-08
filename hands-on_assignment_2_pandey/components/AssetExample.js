import React from 'react';
import { View, Text, Image, StyleSheet } from 'react-native';

import myPhoto from '../assets/profile.jpg';

export default function AssetExample() {
  return (
    <View style={styles.wrapper}>
      <Text style={styles.heading}>
        WELCOME TO THE UNIVERSITY of the CUMBERLANDS{'\n'}
        Course ID: MSCS 533
      </Text>

      <View style={styles.photoPanel}>
        <Image source={myPhoto} style={styles.photo} resizeMode="cover" />
      </View>

      <Text style={styles.bio}>
        Abhishek Pandey is a graduate degree student at UC. Here at UC, Abhishek Pandey is learning
        about AI/ML, Multiplatform App Development and DSA. His ambition is to do something in the AI space
        as the tech world is moving ahead at a fast pace. He is enthusiastic about contributing in advancement
        of space exploration with the help of advanced AI.
      </Text>
    </View>
  );
}

const styles = StyleSheet.create({
  wrapper: {
    backgroundColor: 'white',
    padding: 16,
  },
  heading: {
    textAlign: 'center',
    fontSize: 18,
    fontWeight: '800',
    marginBottom: 16,
  },
  photoPanel: {
    alignItems: 'center',
    justifyContent: 'center',
    paddingVertical: 8,
  },
  photo: {
    width: 160,
    height: 200,
    borderRadius: 8,
  },
  bio: {
    marginTop: 12,
    fontSize: 14,
    lineHeight: 20,
    textAlign: 'center',
  },
});
