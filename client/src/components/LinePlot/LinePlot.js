import { LineChart, Line, XAxis, YAxis, Tooltip } from 'recharts'

/**
 * LinePlot component.
 * 
 * @param {object} props The properties used by the component.
 * @returns {React.ReactElement} LinePlot component.
 */
function LinePlot ({ data, xAxisProperty, yAxisProperty, lineName, yAxisName}) {
  return (
    <LineChart
      width={900}
      height={600}
      data={data}
      margin={{
        top: 5,
        right: 30,
        left: 20,
        bottom: 5,
      }}
    >
      <XAxis
        dataKey={xAxisProperty}
        tickFormatter={(dateStr) => new Date(dateStr).toLocaleDateString()}
      />
      <YAxis label={{ value: `${yAxisName}`, position: 'insideTopLeft', dx: -20, dy: -10 }}/>
      <Tooltip labelFormatter={(dateStr) => new Date(dateStr).toLocaleDateString()}/>
      <Line
        type="monotone"
        dataKey={yAxisProperty}
        name={lineName}
        stroke="#82ca9d"
        strokeWidth={2}
      />
    </LineChart>
  )
}

export default LinePlot
