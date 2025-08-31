import 'package:flutter/material.dart';

void main() => runApp(const MeasuresConverterApp());

class MeasuresConverterApp extends StatelessWidget {
  const MeasuresConverterApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Measures Converter',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        useMaterial3: false,
      ),
      home: const ConverterScreen(),
      debugShowCheckedModeBanner: false,
    );
  }
}

enum UnitCategory { length, weight }

class UnitDef {
  final String key; 
  final String long; 
  final UnitCategory cat;
  final double toBase;

  const UnitDef(this.key, this.long, this.cat, this.toBase);
}

class UnitsRegistry {
  static const all = <UnitDef>[
    UnitDef('meters', 'meters (m)', UnitCategory.length, 1.0),
    UnitDef('kilometers', 'kilometers (km)', UnitCategory.length, 1000.0),
    UnitDef('feet', 'feet (ft)', UnitCategory.length, 0.3048),
    UnitDef('miles', 'miles (mi)', UnitCategory.length, 1609.344),

    UnitDef('kilograms', 'kilograms (kg)', UnitCategory.weight, 1.0),
    UnitDef('grams', 'grams (g)', UnitCategory.weight, 0.001),
    UnitDef('pounds', 'pounds (lb)', UnitCategory.weight, 0.45359237),
    UnitDef('ounces', 'ounces (oz)', UnitCategory.weight, 0.028349523125),
  ];

  static List<UnitDef> byCategory(UnitCategory c) =>
      all.where((u) => u.cat == c).toList(growable: false);
}

class ConverterScreen extends StatefulWidget {
  const ConverterScreen({super.key});
  @override
  State<ConverterScreen> createState() => _ConverterScreenState();
}

class _ConverterScreenState extends State<ConverterScreen> {
  final _valueCtrl = TextEditingController(text: '100'); 
  UnitCategory _category = UnitCategory.length;
  late List<UnitDef> _units;
  late UnitDef _from;
  late UnitDef _to;

  String? _result;

  @override
  void initState() {
    super.initState();
    _units = UnitsRegistry.byCategory(_category);
    _from = _units.first; 
    _to = _units[1]; 
    _compute();
  }

  @override
  void dispose() {
    _valueCtrl.dispose();
    super.dispose();
  }

  void _changeCategory(UnitCategory c) {
    setState(() {
      _category = c;
      _units = UnitsRegistry.byCategory(c);
      _from = _units.first;
      _to = _units[1];
      _compute();
    });
  }

  double _convert(double v, UnitDef from, UnitDef to) {
    final base = v * from.toBase;
    return base / to.toBase;
  }

  void _compute() {
    final raw = _valueCtrl.text.trim();
    if (raw.isEmpty) {
      setState(() => _result = null);
      return;
    }
    final v = double.tryParse(raw);
    if (v == null) {
      setState(() => _result = 'Please enter a valid number.');
      return;
    }
    final out = _convert(v, _from, _to);
    setState(() {
      _result =
          '${v.toStringAsFixed(1)} ${_from.key} are ${out.toStringAsFixed(3)} ${_to.key}';
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Measures Converter')),
      body: SafeArea(
        child: SingleChildScrollView(
          padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              const SizedBox(height: 8),
              const Center(
                child: Text('Value',
                    style: TextStyle(fontSize: 24, fontWeight: FontWeight.w500)),
              ),
              TextField(
                controller: _valueCtrl,
                keyboardType: const TextInputType.numberWithOptions(decimal: true),
                decoration: const InputDecoration(hintText: 'Enter a number'),
                onChanged: (_) => _compute(),
              ),
              const SizedBox(height: 24),

              const Center(
                child: Text('Category',
                    style: TextStyle(fontSize: 24, fontWeight: FontWeight.w500)),
              ),
              const SizedBox(height: 8),
              DropdownButtonFormField<UnitCategory>(
                value: _category,
                items: const [
                  DropdownMenuItem(
                    value: UnitCategory.length,
                    child: Text('Length'),
                  ),
                  DropdownMenuItem(
                    value: UnitCategory.weight,
                    child: Text('Weight'),
                  ),
                ],
                onChanged: (c) => _changeCategory(c ?? UnitCategory.length),
              ),
              const SizedBox(height: 24),

              const Center(
                child: Text('From',
                    style: TextStyle(fontSize: 24, fontWeight: FontWeight.w500)),
              ),
              const SizedBox(height: 8),
              DropdownButtonFormField<UnitDef>(
                value: _from,
                items: _units
                    .map((u) => DropdownMenuItem(value: u, child: Text(u.key)))
                    .toList(),
                onChanged: (u) {
                  if (u != null) setState(() => _from = u);
                  _compute();
                },
              ),
              const SizedBox(height: 24),

              const Center(
                child: Text('To',
                    style: TextStyle(fontSize: 24, fontWeight: FontWeight.w500)),
              ),
              const SizedBox(height: 8),
              DropdownButtonFormField<UnitDef>(
                value: _to,
                items: _units
                    .map((u) => DropdownMenuItem(value: u, child: Text(u.key)))
                    .toList(),
                onChanged: (u) {
                  if (u != null) setState(() => _to = u);
                  _compute();
                },
              ),
              const SizedBox(height: 24),

              Center(
                child: ElevatedButton(
                  onPressed: _compute,
                  child: const Text('Convert'),
                ),
              ),
              const SizedBox(height: 24),

              if (_result != null)
                Center(
                  child: Text(
                    _result!,
                    style: const TextStyle(fontSize: 20, color: Colors.black54),
                    textAlign: TextAlign.center,
                  ),
                ),
            ],
          ),
        ),
      ),
    );
  }
}